namespace TokiMonsi.Palindrome;

class Fragment
{
	Fragment(IImmutableList<string> words, int offset) =>
		(Words, Offset) = (words, offset);

	IImmutableList<string> Words { get; init; }
	int Offset { get; init; }

	public int Length => Words.Count;

	public bool IsComplete => Offset == 0;
	public bool CanPrepend => Offset < 0;
	public bool CanAppend => Offset >= 0;

	public string GetPhrase => string.Join(' ', Words);

	public ReadOnlySpan<char> LooseEnd
	{
		get
		{
			Debug.Assert(CanPrepend);
			var lastWord = Words[^1];
			return lastWord.AsSpan(lastWord.Length + Offset);
		}
	}

	public ReadOnlySpan<char> LooseBeginning
	{
		get
		{
			Debug.Assert(CanAppend);
			var firstWord = Words[0];
			return firstWord.AsSpan(0, Offset);
		}
	}

	public static Fragment? FromSingleWord(string word, int offset)
	{
		var length = word.Length;
		Debug.Assert(-length <= offset && offset < length);

		var matchingPart = offset < 0
			? word.AsSpan(0, length + offset)
			: word.AsSpan(offset);

		return matchingPart.EqualsReversed(matchingPart)
			? new Fragment(ImmutableList.Create(word), offset)
			: null;
	}

	public Fragment Prepend(string word)
	{
		Debug.Assert(CanPrepend);
		Debug.Assert(LooseEnd.EqualsReversed(word));

		return new Fragment(
			Words.Insert(0, word),
			Offset + word.Length);
	}

	public Fragment Append(string word)
	{
		Debug.Assert(CanAppend);
		Debug.Assert(word.EqualsReversed(LooseBeginning));

		return new Fragment(
			Words.Add(word),
			Offset - word.Length);
	}
}
