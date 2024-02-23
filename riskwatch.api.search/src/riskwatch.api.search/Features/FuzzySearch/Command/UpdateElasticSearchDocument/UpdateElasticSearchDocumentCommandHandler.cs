using AutoMapper;
using MediatR;
using riskwatch.api.search.Features.FuzzySearch.Service.ElasticSearchService;
using riskwatch.api.search.Features.FuzzySearch.Service.EntityRecordService;

namespace riskwatch.api.search.Features.FuzzySearch.Command.UpdateElasticSearchDocument;

public class UpdateElasticSearchDocumentCommandHandler : IRequestHandler<UpdateElasticSearchDocumentCommand, bool>
{
    private readonly IElasticSearchService _elasticSearchService;
    private readonly IMapper _mapper;
    private readonly IEntityRecordService _entityRecordService;
    
    public UpdateElasticSearchDocumentCommandHandler(IElasticSearchService elasticSearchService, IMapper mapper, IEntityRecordService entityRecordService)
    {
        _elasticSearchService = elasticSearchService;
        _mapper = mapper;
        _entityRecordService = entityRecordService;
    }

    public async Task<bool> Handle(UpdateElasticSearchDocumentCommand request, CancellationToken cancellationToken)
    {
        var records = await _entityRecordService.GetAndMapEntityRecordsAsync();
        var bulkIndexResponse = await _elasticSearchService.BulkUpsertDocumentsAsync(records);

        if(!bulkIndexResponse)
        {
            throw new Exception("Error occurred while updating the documents in ElasticSearch");
        }
        
        return true;
    }
}