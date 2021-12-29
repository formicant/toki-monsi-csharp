namespace TokiMonsi.Palindrome;

static class SpanExtensions
{
	public static bool EqualsReversed(this ReadOnlySpan<char> forward, ReadOnlySpan<char> backward)
	{
		var length = Min(forward.Length, backward.Length);
		for(int i = 0; i < length; i++)
			if(char.ToLowerInvariant(forward[i]) != char.ToLowerInvariant(backward[^(i + 1)]))
				return false;

		return true;
	}

	public static bool EqualsReversed(this string forward, ReadOnlySpan<char> backward) =>
		forward.AsSpan().EqualsReversed(backward);
}
