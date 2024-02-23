using AutoMapper;
using MediatR;
using riskwatch.api.search.Features.Common.Helper;
using riskwatch.api.search.Features.FuzzySearch.Dto;
using riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;

namespace riskwatch.api.search.Features.FuzzySearch.Queries.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, List<SearchResultDto>>
{
    private readonly IElasticSearchService _elasticSearchService;
    private readonly IMapper _mapper;
    private readonly StringSimilarityComparer _stringSimilarityComparer;
    
    public SearchQueryHandler(IElasticSearchService elasticSearchService, IMapper mapper, StringSimilarityComparer stringSimilarityComparer)
    {
        _elasticSearchService = elasticSearchService;
        _mapper = mapper;
        _stringSimilarityComparer = stringSimilarityComparer;
    }


    public async Task<List<SearchResultDto>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var response = await _elasticSearchService.SearchAsync(request.SearchTerm);
        if (!response.IsValid)
        {
            // Handle the error - the response will contain the details
            Console.WriteLine("Search failed!");
            Console.WriteLine(response.ServerError?.ToString());
            return new List<SearchResultDto>();
        }
        
        var resultsWithPercentage = response.Hits.Select(hit =>
        {
            var dto = _mapper.Map<SearchResultDto>(hit.Source);
            // Calculate the score based on the closeness of the match
            dto.Score = hit.Score ?? 0;
            return dto;
        }).ToList();
        
        var resultsWithSimilarityScores = _stringSimilarityComparer.CalculateSimilarityScores(request.SearchTerm, resultsWithPercentage);
        var filteredResults = resultsWithSimilarityScores
            .Where(dto => dto.RelevanceScore >= request.Threshold)
            .OrderByDescending(dto => dto.RelevanceScore)
            .ToList();
        
        return filteredResults;
    }
}