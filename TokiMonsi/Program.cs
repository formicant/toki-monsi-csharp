namespace TokiMonsi;

static class Program
{
	static void Main()
	{
		var maxWordCount = 8;
		var wordList = Words.PuWords;

		var generator = new Generator();

		var sw = new Stopwatch();
		sw.Start();
		var palindromes = generator.GeneratePalindromes(wordList, maxWordCount);
		sw.Stop();
		var elapsed = sw.Elapsed.TotalSeconds;

		//foreach(var palindrome in palindromes)
		//	WriteLine(palindrome);

		WriteLine();
		WriteLine($"count: {palindromes.Count}");
		WriteLine($"elapsed: {elapsed}");
	}
}
