namespace TokiMonsi.Tests;

public class PalindromeTests
{
	[TestCase(new[] { "a", "ala", "alasa", "kala", "la", "pu" }, 7)]
	public void TestGeneratePalindromes(IReadOnlyList<string> wordList, int maxWordCount)
	{
		var generator = new PalindromesGenerator(wordList);
		var actual = generator.GeneratePalindromes(maxWordCount);
		var expected = GeneratePalindromesNaïvely(wordList, maxWordCount);
		Assert.That(actual, Is.EquivalentTo(expected));
	}

	[Test]
	public void TestCaseInsensitiveness()
	{
		var casedWordList = new[] { "ala", "Ala", "kALa" };
		var expected = new[] { "ala", "Ala", "ala ala", "ala Ala", "Ala ala", "Ala Ala", "ala kALa", "Ala kALa" };
		var generator = new PalindromesGenerator(casedWordList);
		var actual = generator.GeneratePalindromes(2);
		Assert.That(actual, Is.EquivalentTo(expected));
	}

	#region Helper functions

	static IReadOnlyList<string> GeneratePalindromesNaïvely(IReadOnlyList<string> wordList, int maxWordCount) =>
		(
			from wordCount in Enumerable.Range(1, maxWordCount)
			from wordCombination in wordList.CartesianProduct(wordCount)
			where IsPalindrome(string.Join("", wordCombination))
			select string.Join(" ", wordCombination)
		).ToList();

	static bool IsPalindrome(string s) =>
		s.Equals(new string(s.Reverse().ToArray()), StringComparison.InvariantCultureIgnoreCase);

	#endregion
}
