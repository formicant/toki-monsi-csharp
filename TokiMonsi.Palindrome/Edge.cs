namespace TokiMonsi.Palindrome;

/// <summary>
/// Edge between two graph nodes marked with a word.
/// The edge exists iff <c>ToNode</c> is reached by adding <c>Word</c>
/// to fragments from the <c>FromNode</c> class.
/// </summary>
readonly record struct Edge(
	Node FromNode,
	string Word,
	Node ToNode)
{
	public override string ToString() =>
		$"{FromNode} ({Word})→ {ToNode}";
}
