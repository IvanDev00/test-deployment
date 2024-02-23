using Nest;
using riskwatch.api.search.Features.FuzzySearch.Dto;
using riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;

namespace riskwatch.api.search.Features.Service.ElasticSearchService;

public class ElasticSearchService : IElasticSearchService
{
    private readonly IElasticClient _elasticClient;
    public ElasticSearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }
    
    public async Task<bool> CreateIndexAsync()
    {
        var elasticSearchIndexName = "riskwatch-search-v5";
        var createIndexResponse = await _elasticClient.Indices.CreateAsync(elasticSearchIndexName, c => c
            .Map<SearchResultDto>(m => m
                .AutoMap()
                .Properties(props => props
                    .Completion(comp => comp
                        .Name(n => n.Suggest)
                        .Analyzer("simple")
                    )
                )
            )
        );// This will infer the mapping from the IndividualRecordDto class

        if (!createIndexResponse.IsValid)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> IndexDataExistAsync(string indexName)
    {
        var response = await _elasticClient.Indices.ExistsAsync(indexName);
        return response.Exists;
    }

    public async Task<bool> BulkIndexDataAsync(List<SearchResultDto> records)
    {
        const int batchSize = 1000; // Adjust batch size accordingly
        var batches = Enumerable.Range(0, (records.Count + batchSize - 1) / batchSize)
            .Select(i => records.Skip(i * batchSize).Take(batchSize));

        foreach (var batch in batches)
        {
            var bulkDescriptor = new BulkDescriptor();
            foreach (var record in batch)
            {
                // Using RiskwatchNumber as the document identifier for Elasticsearch.
                // Ensure that RiskwatchNumber is not null or empty and is unique across all documents.
                bulkDescriptor.Update<SearchResultDto, object>(u => u
                    .Doc(record)
                    .DocAsUpsert(true)
                    .Id(record.RiskwatchNumber)
                );
            } 
                
            var bulkResponse = await _elasticClient.BulkAsync(bulkDescriptor);
            if (!bulkResponse.IsValid)
            {
                // Log the response to understand what went wrong
                // Optionally, you could throw an exception or handle it as needed
                Console.WriteLine($"Failed to index batch. Error: {bulkResponse.OriginalException.Message}");
                return false;
            }
        }
        return true;
    }

    public async Task<bool> BulkUpsertDocumentsAsync(List<SearchResultDto> records)
    {
        var bulkDescriptor = new BulkDescriptor();

        foreach (var record in records)
        {
            bulkDescriptor.Update<SearchResultDto, object>(u => u
                .Doc(record)
                .DocAsUpsert(true)
                .Id(record.RiskwatchNumber)
            );
        }
        var bulkResponse = await _elasticClient.BulkAsync(bulkDescriptor);

        if (!bulkResponse.IsValid)
        {
            // Log the error or handle it as needed
            Console.WriteLine($"Bulk upsert operation failed. Error: {bulkResponse.OriginalException.Message}");
            return false;
            // Return or handle the failure accordingly
        }
            
        return true;
    }
    
    public async Task<ISearchResponse<SearchResultDto>> SearchAsync(string searchTerm)
    {
        var response = await _elasticClient.SearchAsync<SearchResultDto>(s => s
            .Query(q => q
                .Bool(b=> b
                    .Must(must => must 
                        .MultiMatch(m => m
                            .Fields(f => f
                                .Field(p => p.Name)
                                .Field(p => p.RiskwatchNumber)
                                .Field(p => p.BirthDate)
                                .Field(p => p.Type)
                                .Field(p => p.Aliases.First().FullName)
                                .Field(p => p.Aliases.First().FirstName)
                                .Field(p => p.Aliases.First().LastName)
                                .Field(p => p.Aliases.First().CompleteBusinessName)
                                .Field(p => p.Aliases.First().NameOfBusiness)
                            )
                            .Query(searchTerm)
                            .Fuzziness(Fuzziness.Auto)
                            .Type(TextQueryType.MostFields)
                        ) 
                    )
                )
            )
            .Highlight(h =>h 
                .Fields(f=>f
                    .Field(p => p.Name)
                    .PreTags("<em>")
                    .PostTags("</em>")
                )
            )
        );
        return response;
    }

    public async Task<IEnumerable<string>> GetSearchSuggestionsAsync(string searchTerm)
    {
        var response = await _elasticClient.SearchAsync<SearchResultDto>(s => s
            .Suggest(su => su
                .Completion("suggestion", c => c  // Ensure this name matches with the name used below
                    .Field(p => p.Suggest)  // Your DTO needs a Suggest property of type CompletionField
                    .Prefix(searchTerm)
                    .Fuzzy(f => f.Fuzziness(Fuzziness.Auto))
                    .Size(5)
                )
            )
        );

        // Extract suggestions and their scores
        if (response.Suggest != null && response.Suggest.ContainsKey("suggestion"))
        {
            return response.Suggest["suggestion"] // Ensure this matches the name in the search request
                .SelectMany(x => x.Options)
                .Select(x => x.Text);
        }
        else
        {
            Console.WriteLine("No suggestions found!");
            return Enumerable.Empty<string>();
        }
    }
}