using ConceptNet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConceptNet;

public interface IConceptNetApi
{
    Task<T> LookupAsync<T>(string id, QueryOptions options = default) where T : ConceptNetEntity;

    Task<LinkedDataEntity> GetUriAsync(string language, string text);

    Task<ConceptNetQuery> QueryAsync(QueryOptions options);

    Task<T> GetAsync<T>(string uri, QueryOptions options = default);
}

public static class ConceptNetApiExtensions
{
    public static async Task<ConceptNetEntity> LookupAsync(this IConceptNetApi api, string id, QueryOptions options = default)
    {
        return ConceptNetUtilities.IdentifyEntityType(id) == ConceptNetEntityType.Assertion
            ? await api.LookupAsync<ConceptNetEdge>(id, options)
            : await api.LookupAsync<ConceptNetQuery>(id, options);
    }

    public static async Task<ConceptNetQuery?> QueryNextPageAsync(this IConceptNetApi api, ConceptNetQuery currentQuery)
    {
        if (!currentQuery.TryGetNextPage(out var nextPage))
            return null;

        return await api.GetAsync<ConceptNetQuery>(nextPage);
    }

    public static async IAsyncEnumerable<ConceptNetEdge> QueryAllAsync(this IConceptNetApi api, QueryOptions options)
    {
        ConceptNetQuery results;
        string? nextPage = null;
        do
        {
            results = nextPage is null
                ? await api.QueryAsync(options)
                : await api.GetAsync<ConceptNetQuery>(nextPage, options);

            foreach (var edge in results.Edges)
                yield return edge;
        }
        while (results.TryGetNextPage(out nextPage));
    }
}
