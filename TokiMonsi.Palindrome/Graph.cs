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
	/// <summary>Start node.</summary>
	public StartNode StartNode { get; } = new();

	/// <summary>Edges grouped by their from-nodes.</summary>
	public IReadOnlyDictionary<INode, IReadOnlySet<Edge>> EdgesFromNode { get; }

	/// <summary>Distance from every node to the final node.</summary>
	public IReadOnlyDictionary<INode, int> Distances { get; }

	public Graph(IReadOnlyList<string> wordList)
	{
		var edgesToNode = GetEdges(wordList)
			.GroupBy(edge => edge.ToNode)
			.ToDictionary(g => g.Key, g => g.AsEnumerable());

		Distances = GetDistances(edgesToNode);

		// Group edges by from-node leaving only useful ones
		EdgesFromNode = edgesToNode
			.Where(p => Distances.ContainsKey(p.Key))
			.SelectMany(p => p.Value)
			.GroupBy(edge => edge.FromNode)
			.ToDictionary(g => g.Key, g => g.ToHashSet() as IReadOnlySet<Edge>);
	}


	/// <summary>
	/// Returns all graph edges.
	/// </summary>
	static IEnumerable<Edge> GetEdges(IReadOnlyList<string> wordList)
	{
		var tailNodes = new HashSet<TailNode>();
		var edgesToNode = new Dictionary<TailNode, HashSet<Edge>>();

		// Add start edges
		foreach (var word in wordList)
		{
			var length = word.ToLowerInvariant().Length;

			foreach (var offset in Enumerable.Range(-length, 2 * length))
			{
				var (node, possible_edge) = Edge.TryStart(word, offset);
				tailNodes.Add(node);

				// A node is accessible from start if the word's matching part is palindromic
				if (possible_edge is Edge edge)
					yield return edge;
			}
		}

		// Add other edges
		var possible_edges =
			from fromNode in tailNodes
			from word in wordList
			select Edge.TryCreate(fromNode, word);
		var edges = possible_edges.OfType<Edge>();

		foreach (var edge in edges)
			yield return edge;
	}

	/// <summary>
	/// Finds distance from every node to the final node.
	/// </summary>
	static IReadOnlyDictionary<INode, int> GetDistances(IReadOnlyDictionary<TailNode, IEnumerable<Edge>> edgesToNode)
	{
		var finalNode = new TailNode("", 0);
		var distances = new Dictionary<INode, int> { [finalNode] = 0 };

		var queue = new PriorityQueue<TailNode, int>();
		queue.Enqueue(finalNode, 0);

		while (queue.Count > 0)
		{
			var node = queue.Dequeue();
			var fromNodeDistance = distances[node] + 1;

			if (edgesToNode.TryGetValue(node, out var edges))
				foreach (var edge in edges)
					if (!distances.TryGetValue(edge.FromNode, out var distance) || distance > fromNodeDistance)
					{
						distances[edge.FromNode] = fromNodeDistance;
						if (edge.FromNode is TailNode fromNode)
							queue.Enqueue(fromNode, fromNodeDistance);
					}
		}

		return distances;
	}
}
