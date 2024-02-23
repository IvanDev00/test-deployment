using AutoMapper;
using MediatR;
using riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;

namespace riskwatch.api.search.Features.FuzzySearch.Queries.SearchSuggestion;

public class SearchSuggestionQueryHandler : IRequestHandler<SearchSuggestionQuery, IEnumerable<string>>
{
    private readonly IElasticSearchService _elasticSearchService;
    private readonly IMapper _mapper;
    
    public SearchSuggestionQueryHandler(IElasticSearchService elasticSearchService, IMapper mapper)
    {
        _elasticSearchService = elasticSearchService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<string>> Handle(SearchSuggestionQuery request, CancellationToken cancellationToken)
    {
        return await _elasticSearchService.GetSearchSuggestionsAsync(request.SearchTerm);
    }
}