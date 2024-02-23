using Quartz;
using riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;
using riskwatch.api.search.Features.FuzzySearch.Service.EntityRecordService;

namespace riskwatch.api.search.Features.FuzzySearch.Jobs;

public class BulkUpsertJob : IJob
{
    private readonly IElasticSearchService _elasticSearchService;
    private readonly IEntityRecordService _entityRecordService;
    public BulkUpsertJob(IElasticSearchService elasticSearchService, IEntityRecordService entityRecordService)
    {
        _elasticSearchService = elasticSearchService;
        _entityRecordService = entityRecordService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var records = await _entityRecordService.GetAndMapEntityRecordsAsync();

        if (records.Any())
        {
            bool isSuccessful = await _elasticSearchService.BulkUpsertDocumentsAsync(records);

            if (!isSuccessful)
            {
                Console.WriteLine("Error occurred while updating the documents in ElasticSearch");
                throw new Exception("Error occurred while updating the documents in ElasticSearch");
            }
        }
    }
}