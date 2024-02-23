using Nest;
using Aliases = riskwatch.api.search.Entities.Aliases;

namespace riskwatch.api.search.Features.FuzzySearch.Dto;

public class SearchResultDto
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? BirthDate { get; set; }
    public string? RiskwatchNumber { get; set; }
    public List<Aliases>? Aliases { get; set; }
    public double? Score { get; set; }
    public double? RelevanceScore { get; set; } 
    public CompletionField? Suggest { get; set; } // Add this property for the suggester
}