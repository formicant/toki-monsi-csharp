namespace TokiMonsi.Palindrome;

/// <summary>
/// Edge between two graph nodes marked with a word.
/// The edge exists iff <c>ToNode</c> is reached by adding <c>Word</c>
/// to fragments from the <c>FromNode</c> class.
/// </summary>
readonly record struct Edge(
	INode FromNode,
	string Word,
	TailNode ToNode)
{
	public override string ToString() =>
		$"{FromNode} ({Word})→ {ToNode}";

	/// <summary>
	/// Creates an edge by its <paramref name="fromNode"/> and <paramref name="word" />
	/// or returns <c>null</c> if impossible.
	/// </summary>
	public static Edge? TryCreate(TailNode fromNode, string word)
	{
		var caselessWord = word.ToLowerInvariant();

		var toNodeOffset = fromNode.Offset - Sign(fromNode.Offset) * caselessWord.Length;
		var wordOffset = -Sign(toNodeOffset) * word.Length;

		if (Sign(fromNode.Offset) == Sign(toNodeOffset))
		{
			var (toNodeTail, tailMatchingPart) = SliceByOffset(fromNode.Tail, wordOffset);
			return AreEqualReversed(tailMatchingPart, caselessWord)
				? new Edge(fromNode, word, new TailNode(toNodeTail, toNodeOffset))
				: null;
		}
		else
		{
			var (toNodeTail, wordMatchingPart) = SliceByOffset(caselessWord, fromNode.Offset);
			return AreEqualReversed(fromNode.Tail, wordMatchingPart)
				? new Edge(fromNode, word, new TailNode(toNodeTail, toNodeOffset))
				: null;
		}
	}

	public static (TailNode, Edge?) TryStart(string word, int offset)
	{
		var caselessWord = word.ToLowerInvariant();

		var (matchingPart, tail) = SliceByOffset(caselessWord, offset);
		var node = new TailNode(tail, offset);

		// A node is accessible from start if the word's matching part is palindromic
		Edge? edge = AreEqualReversed(matchingPart, matchingPart)
			? new Edge(new StartNode(), word, node)
			: null;

		return (node, edge);
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
