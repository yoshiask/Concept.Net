using ConceptNet.Models;
using System.Linq;
using Xunit.Abstractions;

namespace ConceptNet.Tests;

public class RestApiTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    private readonly IConceptNetApi _api = new ConceptNetRestApi();

    [Theory]
    [InlineData("/c/en/night")]
    [InlineData("/c/cop/ⲉϫⲱⲣϩ")]
    [InlineData("/c/ar/ليل/n/wn/time")]
    [InlineData("/r/FormOf")]
    [InlineData("/d/wiktionary/en")]
    [InlineData("/s/resource/wiktionary/en")]
    [InlineData("/s/process/wikiparsec/2")]
    [InlineData("/a/[/r/FormOf/,/c/cop/ϣⲉⲡ/v/,/c/cop/ϣⲱⲡ/]")]
    public async Task Lookup(string id)
    {
        var result = await _api.LookupAsync(id);

        if (result.Error is not null)
            Assert.Fail(result.Error.ToString());

        Assert.NotNull(result);
        Assert.NotNull(result.Id);

        IEnumerable<ConceptNetEdge> edges = result switch
        {
            ConceptNetEdge edge => [edge],
            ConceptNetQuery query => query.Edges,
            _ => throw new ArgumentException("Result was an unexpected type")
        };

        foreach (var edge in edges)
            _output.WriteLine($"{edge.Id}:\t{edge.SurfaceText}");
    }

    [Theory]
    [InlineData("en", "night")]
    [InlineData("cop", "ⲉϫⲱⲣϩ")]
    [InlineData("cop", "ⲓⲱⲧ")]
    [InlineData("cop", "ⲓⲱϯ")]
    public async Task ResolveUri(string language, string text)
    {
        var result = await _api.GetUriAsync(language, text);

        Assert.NotNull(result);
        Assert.NotNull(result.Id);

        _output.WriteLine(result.Id);
    }

    [Theory]
    [InlineData("/c/en/night")]
    [InlineData("/c/cop/ⲉϫⲱⲣϩ")]
    [InlineData("/c/ar/ليل/n/wn/time")]
    [InlineData("/r/FormOf")]
    public async Task QueryAll(string id)
    {
        QueryOptions options = new();

        switch (ConceptNetUtilities.IdentifyEntityType(id))
        {
            case ConceptNetEntityType.Relation:
                options.Relation = id;
                break;

            default:
                options.Node = id;
                break;
        }

        await foreach (var edge in _api.QueryAllAsync(options).Take(100))
            _output.WriteLine(edge.ToString());
    }
}