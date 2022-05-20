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

		var sw = new Stopwatch();
		sw.Start();
		var generator = new PalindromesGenerator(wordList);
		sw.Stop();
		var graph_time = sw.Elapsed;

		sw.Restart();
		var palindromes = generator.GeneratePalindromes(maxWordCount);
		sw.Stop();
		var generation_time = sw.Elapsed;

		//foreach(var palindrome in palindromes)
		//	WriteLine(palindrome);

		WriteLine();
		WriteLine($"count: {palindromes.Count}");
		WriteLine($"        graph building: {graph_time}");
		WriteLine($" palindrome generation: {generation_time}");
	}
}
