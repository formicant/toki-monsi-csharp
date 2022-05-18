namespace TokiMonsi;

using System.Diagnostics;
using static System.Console;
using TokiMonsi.Palindrome;

static class Program
{
	static void Main()
	{
		var maxWordCount = 9;
		var wordList = Words.PuWords;

		var generator = new PalindromesGenerator(wordList);

		var sw = new Stopwatch();
		sw.Start();
		var palindromes = generator.GeneratePalindromes(maxWordCount);
		sw.Stop();
		var elapsed = sw.Elapsed.TotalSeconds;

		//foreach(var palindrome in palindromes)
		//	WriteLine(palindrome);

		WriteLine();
		WriteLine($"count: {palindromes.Count}");
		WriteLine($"elapsed: {elapsed}");
	}
}
