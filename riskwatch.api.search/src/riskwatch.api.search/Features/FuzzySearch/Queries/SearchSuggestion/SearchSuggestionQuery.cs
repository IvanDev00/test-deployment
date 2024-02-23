using MediatR;

namespace riskwatch.api.search.Features.FuzzySearch.Queries.SearchSuggestion;

public class SearchSuggestionQuery : IRequest<IEnumerable<string>>
{
    public string SearchTerm { get; set; } = string.Empty;
}