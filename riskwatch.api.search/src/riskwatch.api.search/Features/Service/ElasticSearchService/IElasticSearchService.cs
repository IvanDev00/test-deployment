using Nest;
using riskwatch.api.search.Features.FuzzySearch.Dto;

namespace riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;

public interface IElasticSearchService
{
    Task<bool> CreateIndexAsync();
    Task<bool> IndexDataExistAsync(string indexName);
    Task<ISearchResponse<SearchResultDto>> SearchAsync(string searchTerm);
    Task<IEnumerable<string>> GetSearchSuggestionsAsync(string searchTerm);
    Task<bool> BulkUpsertDocumentsAsync(List<SearchResultDto> records);
    Task<bool> BulkIndexDataAsync(List<SearchResultDto> records);
}