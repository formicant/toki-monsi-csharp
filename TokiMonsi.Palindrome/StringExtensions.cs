namespace TokiMonsi.Palindrome;

static class StringExtensions
{
	public static bool EqualsReversed(this string forward, string backward) =>
		Enumerable.Zip(forward, backward.Reverse())
			.All(t => char.ToLowerInvariant(t.First) == char.ToLowerInvariant(t.Second));
}
