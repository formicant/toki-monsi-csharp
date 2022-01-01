namespace TokiMonsi.Palindrome;

record Fragment(IImmutableList<string> Words, int Offset)
{
	public bool IsComplete => Offset == 0;

	public string Phrase => string.Join(' ', Words);

	public string LooseEnd
	{
		get
		{
			Debug.Assert(Offset < 0);
			var lastWord = Words[^1];
			return lastWord[^-Offset..];
		}
	}

	public string LooseBeginning
	{
		get
		{
			Debug.Assert(Offset >= 0);
			var firstWord = Words[0];
			return firstWord[..Offset];
		}
	}

	public static Fragment? FromSingleWord(string word, int offset)
	{
		Debug.Assert(-word.Length <= offset && offset < word.Length);

		var matchingPart = offset < 0
			? word[..^-offset]
			: word[offset..];

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
