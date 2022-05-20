namespace TokiMonsi.Palindrome;

/// <summary>
/// Helper graph for building palindromes.
/// To build a palindrome, one should start with the start node
/// and go along the edges till the final node.
/// At each step, the word from the edge should be added to the fragment
/// at the opposite side from the fragment's tail.
/// </summary>
class Graph
{
	/// <summary>Start edges.</summary>
	public IReadOnlyList<StartEdge> StartEdges { get; }

	/// <summary>Edges grouped by their from-nodes.</summary>
	public ILookup<Node, Edge> EdgesFromNode { get; }

	/// <summary>Distance from every node to the final node.</summary>
	public IReadOnlyDictionary<Node, int> Distances { get; }

	public Graph(IReadOnlyList<string> wordList)
	{
		var startEdges = GetStartEdges(wordList).ToList();
		var edges = GetEdges(startEdges, wordList).ToList();

		Distances = CalculateDistances(edges);

		// Leave only useful start edges
		StartEdges = startEdges
			.Where(edge => Distances.ContainsKey(edge.ToNode))
			.ToList();

		// Group edges by from-node leaving only useful ones
		EdgesFromNode = edges
			.Where(edge => Distances.ContainsKey(edge.ToNode))
			.ToLookup(edge => edge.FromNode);
	}

	static IEnumerable<StartEdge> GetStartEdges(IReadOnlyList<string> wordList) =>
		from word in wordList
		let caselessWord = word.ToLowerInvariant()
		from offset in Enumerable.Range(-caselessWord.Length, 2 * caselessWord.Length)
		let toNode = TryCreateStartNode(caselessWord, offset)
		where toNode is not null
		select new StartEdge(word, toNode.Value);

	static IEnumerable<Edge> GetEdges(IEnumerable<StartEdge> startEdges, IReadOnlyList<string> wordList)
	{
		var nodes = startEdges
			.Select(edge => edge.ToNode)
			.ToHashSet();

		var queue = new Queue<Node>(nodes);
		while (queue.Count > 0)
		{
			var fromNode = queue.Dequeue();
			foreach (var word in wordList)
			{
				var caselessWord = word.ToLowerInvariant();

				if (TryCreateNode(fromNode, caselessWord) is Node toNode)
				{
					yield return new Edge(fromNode, word, toNode);

					if (!nodes.Contains(toNode))
					{
						nodes.Add(toNode);
						queue.Enqueue(toNode);
					}
				}
			}
		}
	}

	/// <summary>
	/// Finds distance from every node to the final node.
	/// </summary>
	static IReadOnlyDictionary<Node, int> CalculateDistances(IEnumerable<Edge> edges)
	{
		var fromNodesByToNode = edges
			.ToLookup(edge => edge.ToNode, edge => edge.FromNode);

		var finalNode = new Node("", 0);
		var distances = new Dictionary<Node, int> { [finalNode] = 0 };
		
		var queue = new PriorityQueue<Node, int>();
		queue.Enqueue(finalNode, 0);

		while (queue.Count > 0)
		{
			var toNode = queue.Dequeue();
			var fromNodeDistance = distances[toNode] + 1;

			foreach (var fromNode in fromNodesByToNode[toNode])
				if (!distances.TryGetValue(fromNode, out int distance) || distance > fromNodeDistance)
				{
					distances[fromNode] = fromNodeDistance;
					queue.Enqueue(fromNode, fromNodeDistance);
				}
		}

		return distances;
	}

	/// <summary>
	/// Creates a node if it is accessible form start by <paramref name="word" /> and <paramref name="offset" />
	/// or returns <c>null</c> if impossible.
	/// </summary>
	public static Node? TryCreateStartNode(string caselessWord, int offset)
	{
		var (matchingPart, tail) = SliceByOffset(caselessWord, offset);

		// A node is accessible from start if the word's matching part is palindromic
		return AreEqualReversed(matchingPart, matchingPart)
			? new Node(tail, offset)
			: null;
	}

	/// <summary>
	/// Creates a node if it is accessible form <paramref name="fromNode"/> by <paramref name="word" />
	/// or returns <c>null</c> if impossible.
	/// </summary>
	public static Node? TryCreateNode(Node fromNode, string caselessWord)
	{
		var toNodeOffset = fromNode.Offset - Sign(fromNode.Offset) * caselessWord.Length;
		var wordOffset = -Sign(toNodeOffset) * caselessWord.Length;

		if (Sign(fromNode.Offset) == Sign(toNodeOffset))
		{
			var (toNodeTail, tailMatchingPart) = SliceByOffset(fromNode.Tail, wordOffset);
			return AreEqualReversed(tailMatchingPart, caselessWord)
				? new Node(toNodeTail, toNodeOffset)
				: null;
		}
		else
		{
			var (toNodeTail, wordMatchingPart) = SliceByOffset(caselessWord, fromNode.Offset);
			return AreEqualReversed(fromNode.Tail, wordMatchingPart)
				? new Node(toNodeTail, toNodeOffset)
				: null;
		}
	}

	static (string, string) SliceByOffset(string word, int offset) =>
		offset >= 0
			? (word[offset..], word[..offset])
			: (word[..(word.Length + offset)], word[(word.Length + offset)..]);

	static bool AreEqualReversed(string forward, string backward) =>
		Enumerable.Zip(forward, backward.Reverse())
			.All(t => t.First == t.Second);

	static int Sign(int offset) =>
		offset >= 0 ? 1 : -1;
}
