namespace TokiMonsi.Tests;

static class Extensions
{
	public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
	{
		IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
		return sequences.Aggregate(
			emptyProduct,
			(accumulator, sequence) =>
				from accSeq in accumulator
				from item in sequence
				select accSeq.Concat(new[] { item }));
	}

	public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<T> sequence, int dimensions) =>
		Enumerable.Range(0, dimensions).Select(_ => sequence).CartesianProduct();

	public static Node? ParseNode(this string? notation)
	{
		if (notation is null)
			return null;
		var length = notation.Length - 1;
		if (notation.StartsWith('-'))
			return new Node(notation[1..], -length);
		if (notation.EndsWith('-'))
			return new Node(notation[..length], length);
		throw new ArgumentException("Incorrect node notaion", nameof(notation));
	}
}
