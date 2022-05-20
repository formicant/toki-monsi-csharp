namespace TokiMonsi.Tests;

public class GraphTests
{
    [TestCase("-",     "en",    "-en" )]
    [TestCase("oli-",  "ilo",   "-"   )]
    [TestCase("-in",   "ni",    "-"   )]
    [TestCase("utal-", "la",    "ut-" )]
    [TestCase("-insa", "ni",    "-sa" )]
    [TestCase("ka-",   "akesi", "-esi")]
    [TestCase("-ni",   "olin",  "ol-" )]
    [TestCase("wa-",   "awen",  "-en" )]
    [TestCase("-eli",  "wile",  "w-"  )]
    [TestCase("a-",    "e",     null  )]
    [TestCase("-wa",   "awen",  null  )]
    public void TestTryCreateNode(string from, string word, string? expectedTo)
    {
        var fromNode = from.ParseNode()!.Value;
        var actualToNode = Graph.TryCreateNode(fromNode, word.ToLowerInvariant());
        var expectedToNode = expectedTo.ParseNode();
        Assert.That(actualToNode, Is.EqualTo(expectedToNode));
    }
}
