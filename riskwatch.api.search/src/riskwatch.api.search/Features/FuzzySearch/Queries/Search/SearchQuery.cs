using MediatR;
using riskwatch.api.search.Features.FuzzySearch.Dto;

namespace riskwatch.api.search.Features.FuzzySearch.Queries.Search;

public class SearchQuery : IRequest<List<SearchResultDto>>

{
    public SearchQuery(string searchTerm, int threshold = 85)
    {
        SearchTerm = searchTerm;
        Threshold = threshold;
    }

    public string SearchTerm { get; }
    public int Threshold { get; } 
}