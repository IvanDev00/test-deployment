using F23.StringSimilarity;
using riskwatch.api.search.Features.FuzzySearch.Dto;

namespace riskwatch.api.search.Features.Common.Helper;

public class StringSimilarityComparer
{
    public List<SearchResultDto> CalculateSimilarityScores(string searchTerm, List<SearchResultDto> searchResults)
    {
        // Use the JaroWinkler algorithm to calculate the similarity score
        var jw = new JaroWinkler();
        var lowerCaseSearchTerm = searchTerm.ToLowerInvariant();       
        
        foreach (var result in searchResults)
        {
            var allStrings = new List<string> { result.Name.ToLowerInvariant(), result.RiskwatchNumber.ToLowerInvariant() };
            allStrings.AddRange(result.Aliases.SelectMany(a => new List<string>
            {
                a.LastName?.ToLowerInvariant(),
                a.FirstName?.ToLowerInvariant(),
                a.MiddleName?.ToLowerInvariant(),
                a.CompleteBusinessName?.ToLowerInvariant(),
                a.NameOfBusiness?.ToLowerInvariant(),
                a.FullName?.ToLowerInvariant(),
            }).Where(s => !string.IsNullOrEmpty(s)));

            result.RelevanceScore = Math.Round(allStrings.Max(s => jw.Similarity(lowerCaseSearchTerm, s)) * 100);
        }

        return searchResults;
    }
}