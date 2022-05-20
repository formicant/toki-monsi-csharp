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

	//IReadOnlyList<IReadOnlyList<string>> GetPalindromicSequences(StartEdge startEdge, int maxWordCount)
	//{
	//	var results = new List<IReadOnlyList<string>>();
	//	AddPalindromicSequencesRecursively(startEdge.ToNode, maxWordCount - 1, ImmutableArray.Create(startEdge.Word), results);
	//	return results;
	//}

	//void AddPalindromicSequencesRecursively(Node node, int wordsLeft, ImmutableArray<string> words, IList<IReadOnlyList<string>> result)
	//{
	//	if (_graph.Distances.TryGetValue(node, out var distance) && distance <= wordsLeft)
	//	{
	//		if (distance == 0)
	//			result.Add(words);

	//		if (wordsLeft > 0)
	//			foreach (var edge in _graph.EdgesFromNode[node])
	//			{
	//				var newWords = node.Offset >= 0
	//					? words.Add(edge.Word)
	//					: words.Insert(0, edge.Word);

	//				AddPalindromicSequencesRecursively(edge.ToNode, wordsLeft - 1, newWords, result);
	//			}
	//	}
	//}

	IEnumerable<IReadOnlyList<string>> GetPalindromicSequences(StartEdge startEdge, int maxWordCount)
	{
		var queue = new PriorityQueue<(Node, int, ImmutableArray<string>), int>();
		queue.Enqueue((startEdge.ToNode, maxWordCount - 1, ImmutableArray.Create(startEdge.Word)), maxWordCount);

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
