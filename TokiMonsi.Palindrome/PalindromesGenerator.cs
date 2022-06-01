namespace TokiMonsi.Palindrome;

using System.Collections.Immutable;
using System.Linq;

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
		_graph.StartEdges
			.AsParallel()
			.SelectMany(startEdge => GetPalindromicSequences(startEdge, maxWordCount)
				.Select(words => string.Join(" ", words)))
			.ToList();

	IEnumerable<IReadOnlyList<string>> GetPalindromicSequences(StartEdge startEdge, int maxWordCount)
	{
		var stack = new Stack<Position>();
		stack.Push(new(startEdge.ToNode, maxWordCount - 1, ImmutableArray.Create(startEdge.Word)));

		while (stack.Count > 0)
		{
			var (node, wordsLeft, words) = stack.Pop();

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

						stack.Push(new(edge.ToNode, wordsLeft - 1, newWords));
					}
			}
		}
	}

	readonly record struct Position(
		Node Node,
		int WordsLeft,
		ImmutableArray<string> Words
	);
}
