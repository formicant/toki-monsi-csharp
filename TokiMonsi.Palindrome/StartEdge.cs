namespace TokiMonsi.Palindrome;

readonly record struct StartEdge(
	string Word,
	Node ToNode)
{
	public override string ToString() =>
		$"({Word})→ {ToNode}";
}
