namespace TokiMonsi.Palindrome;

record Fragment(IImmutableList<string> Words, int Offset)
{
	public bool IsComplete => Offset == 0;

	public string Phrase => string.Join(' ', Words);

	public ReadOnlySpan<char> LooseEnd
	{
		get
		{
			Debug.Assert(Offset < 0);
			var lastWord = Words[^1];
			return lastWord.AsSpan(lastWord.Length + Offset);
		}
	}

	public ReadOnlySpan<char> LooseBeginning
	{
		get
		{
			Debug.Assert(Offset >= 0);
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
		Debug.Assert(Offset < 0);
		Debug.Assert(LooseEnd.EqualsReversed(word));

		return new Fragment(
			Words.Insert(0, word),
			Offset + word.Length);
	}

	public Fragment Append(string word)
	{
		Debug.Assert(Offset >= 0);
		Debug.Assert(word.EqualsReversed(LooseBeginning));

		return new Fragment(
			Words.Add(word),
			Offset - word.Length);
	}
}
