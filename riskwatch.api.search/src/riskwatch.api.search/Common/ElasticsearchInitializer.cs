using riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;

namespace riskwatch.api.search.Common;

public class ElasticsearchInitializer
{

    private readonly IElasticSearchService _elasticSearchService;
    
    public ElasticsearchInitializer(IElasticSearchService elasticSearchService)
    {
        _elasticSearchService = elasticSearchService;
    }
    
    public async Task CreateIndexIfNotExistsAsync()
    {
       var indexExist = await _elasticSearchService.IndexDataExistAsync("riskwatch-search-v5");

       if (!indexExist)
       {
           await _elasticSearchService.CreateIndexAsync();
           Console.WriteLine("Index created successfully!");
       }
    }
}