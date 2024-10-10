using ConceptNet.Models;
using Flurl;
using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace ConceptNet;

public class ConceptNetRestApi(Uri apiUri) : IConceptNetApi
{
    private readonly Uri _apiUri = apiUri;

    public Uri ApiUri => _apiUri;

    public ConceptNetRestApi(string uri) : this(new Uri(uri)) { }

    public ConceptNetRestApi() : this(new Uri("https://api.conceptnet.io")) { }

    public async Task<LinkedDataEntity> GetUriAsync(string language, string text)
    {
        return await GetBase("uri", default)
            .AppendQueryParam("language", language)
            .AppendQueryParam("text", text)
            .GetJsonAsync<LinkedDataEntity>();
    }

    public async Task<TEntity> LookupAsync<TEntity>(string id, QueryOptions options = default) where TEntity : ConceptNetEntity
    {
        var response = await GetBase(id, options)
            .GetJsonAsync<TEntity>();

        if (response.Error is not null)
            throw new ConceptNetException(response.Error);

        return response;
    }

    public async Task<ConceptNetQuery> QueryAsync(QueryOptions options)
    {
        var response = await GetBase("query", options)
            .GetJsonAsync<ConceptNetQuery>();

        if (response.Error is not null)
            throw new ConceptNetException(response.Error);

        return response;
    }

    public async Task<T> GetAsync<T>(string uri, QueryOptions options = default)
    {
        return await GetBase(uri, options)
            .GetJsonAsync<T>();
    }

    private Url GetBase(string uri, QueryOptions options)
    {
        var url = Url.Combine(_apiUri.AbsoluteUri, uri);

        if (options.Start is not null)
            url = url.SetQueryParam("start", options.Start);

        if (options.End is not null)
            url = url.SetQueryParam("end", options.End);

        if (options.Node is not null)
            url = url.SetQueryParam("node", options.Node);

        if (options.Other is not null)
            url = url.SetQueryParam("other", options.Other);

        if (options.Relation is not null)
            url = url.SetQueryParam("rel", options.Relation);

        if (options.Limit is not null)
            url = url.SetQueryParam("limit", options.Limit);

        return url;
    }
}
