namespace TokiMonsi.Palindrome;

/// <summary>
/// Graph node representing a class of palindrome fragments with a common tail.
/// </summary>
/// <param name="Tail">Common tail of the palindrome fragments class represented by the node.</param>
/// <param name="Offset">
/// Signed length of the <paramref name="Tail" />.
/// If the tail is located before the pallindromic part, offset is >= 0.
/// If the tail is located after the pallindromic part, offset is < 0.
/// </param>
readonly record struct Node(
	string Tail,
	int Offset)
{
	public override string ToString() => Offset > 0
		? $"{Tail}-"
		: $"-{Tail}";
}
