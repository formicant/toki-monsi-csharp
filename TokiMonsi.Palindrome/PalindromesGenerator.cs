namespace TokiMonsi.Palindrome;

using System.Collections.Immutable;

/// <summary>
/// Generates palindromes using words from <c>wordList</c>.
/// </summary>
public class PalindromesGenerator
{
	public PalindromesGenerator(IReadOnlyList<string> wordList)
	{
		_graph = new Graph(wordList);
	}

	Graph _graph;

	public IReadOnlyList<string> GeneratePalindromes(int maxWordCount) =>
		GetPalindromicSequences(maxWordCount)
			.Select(words => string.Join(" ", words))
			.ToList();

	IEnumerable<IReadOnlyList<string>> GetPalindromicSequences(int maxWordCount)
	{
		var queue = new PriorityQueue<(INode, int, IImmutableList<string>), int>();
		queue.Enqueue((_graph.StartNode, maxWordCount, ImmutableList<string>.Empty), maxWordCount);

		while (queue.Count > 0)
		{
			var (node, wordsLeft, words) = queue.Dequeue();

			if (_graph.Distances.TryGetValue(node, out var distance) && distance <= wordsLeft)
			{
				if (distance == 0)
					yield return words;
				
				if (wordsLeft > 0)
					foreach (var edge in _graph.EdgesFromNode[node])
					{
						var newWords = node.Offset >= 0
							? words.Add(edge.Word)
							: words.Insert(0, edge.Word);

						queue.Enqueue((edge.ToNode, wordsLeft - 1, newWords), wordsLeft);
					}
			}
		}
	}
}
