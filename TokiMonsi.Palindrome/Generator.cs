namespace TokiMonsi.Palindrome;

public class Generator
{
	ConcurrentDictionary<string, IReadOnlyList<string>> _wordsForPrepending = new();
	ConcurrentDictionary<string, IReadOnlyList<string>> _wordsForAppending = new();

	public IReadOnlyList<string> GeneratePalindromes(IReadOnlyList<string> wordList, int maxWordCount)
	{
		IReadOnlyList<string> MakeWordsForPrepending(string matchingPart) =>
			wordList.Where(word => matchingPart.EqualsReversed(word)).ToList();

		IReadOnlyList<string> MakeWordsForAppending(string matchingPart) =>
			wordList.Where(word => word.EqualsReversed(matchingPart)).ToList();

		IReadOnlyList<string> GetWordsForPrepending(string matchingPart) =>
			_wordsForPrepending.GetOrAdd(matchingPart, MakeWordsForPrepending);

		IReadOnlyList<string> GetWordsForAppending(string matchingPart) =>
			_wordsForAppending.GetOrAdd(matchingPart, MakeWordsForAppending);

		IEnumerable<Fragment> GetPalindromesRecursively(Fragment fragment)
		{
			if(fragment.IsComplete)
				yield return fragment;

			if(fragment.Words.Count < maxWordCount)
			{
				var extensions = fragment.Offset switch
				{
					< 0 => GetWordsForPrepending(fragment.LooseEnd.ToString()).Select(fragment.Prepend),
					>= 0 => GetWordsForAppending(fragment.LooseBeginning.ToString()).Select(fragment.Append),
				};
				
				foreach(var extension in extensions)
					foreach(var extensionFragment in GetPalindromesRecursively(extension))
						yield return extensionFragment;
			}
		}

		var cores = wordList
			.SelectMany(word =>
				Enumerable.Range(-word.Length, word.Length * 2)
					.Select(offset => Fragment.FromSingleWord(word, offset))
					.OfType<Fragment>());

		var palindromes = cores
			.AsParallel()
			.SelectMany(GetPalindromesRecursively)
			.Select(p => p.Phrase);

		return palindromes.ToList();
	}
}
