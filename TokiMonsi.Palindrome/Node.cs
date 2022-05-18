namespace TokiMonsi.Palindrome;

/// <summary>
/// Graph node.
/// </summary>
interface INode
{
	int Offset { get; }
}

/// <summary>
/// Graph start node.
/// </summary>
readonly record struct StartNode : INode
{
	public int Offset => 0;

	public override string ToString() => "[start]";
}

/// <summary>
/// Graph node representing a class of palindrome fragments with a common tail.
/// </summary>
/// <param name="Tail">Common tail of the palindrome fragments class represented by the node.</param>
/// <param name="Offset">
/// Signed length of the <paramref name="Tail" />.
/// If the tail is located before the pallindromic part, offset is >= 0.
/// If the tail is located after the pallindromic part, offset is < 0.
/// </param>
readonly record struct TailNode(string Tail, int Offset) : INode
{
	public override string ToString() => Offset > 0
		? $"{Tail}-"
		: $"-{Tail}";
}
