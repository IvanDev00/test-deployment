using AutoMapper;
using MediatR;
using riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;
using riskwatch.api.search.Features.FuzzySearch.Service.EntityRecordService;

namespace riskwatch.api.search.Features.FuzzySearch.Command.BulkDocumentImport;

public class BulkDocumentImportCommandHandler : IRequestHandler<BulkDocumentImportCommand, bool>
{
    private readonly IElasticSearchService _elasticSearchService;
    private readonly IMapper _mapper;
    private readonly IEntityRecordService _entityRecordService;
    
    public BulkDocumentImportCommandHandler(IElasticSearchService elasticSearchService, IMapper mapper, IEntityRecordService entityRecordService)
    {
        _elasticSearchService = elasticSearchService;
        _mapper = mapper;
        _entityRecordService = entityRecordService;
    }
    
    public async Task<bool> Handle(BulkDocumentImportCommand request, CancellationToken cancellationToken)
    {
        var records = await _entityRecordService.GetAndMapEntityRecordsAsync();
        // Attempt to bulk index the records
        var bulkIndexResponse = await _elasticSearchService.BulkIndexDataAsync(records);
        
        if(!bulkIndexResponse)
        {
            throw new Exception("Failed to bulk index records");
        }
        return true;
    }
}