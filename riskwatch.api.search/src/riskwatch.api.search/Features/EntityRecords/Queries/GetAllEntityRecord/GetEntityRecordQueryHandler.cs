using AutoMapper;
using MediatR;
using riskwatch.api.search.Features.Common.Dto;
using riskwatch.api.search.Features.FuzzySearch.Service.EntityRecordService;

namespace riskwatch.api.search.Features.EntityRecords.Queries.GetAllEntityRecord;

public class GetEntityRecordQueryHandler: IRequestHandler<GetEntityRecordQuery, List<EntityRecordDto>>
{
    private readonly IEntityRecordService _entityRecordService;
    private readonly IMapper _mapper;
    
    public GetEntityRecordQueryHandler(IEntityRecordService entityRecordService, IMapper mapper)
    {
        _entityRecordService = entityRecordService;
        _mapper = mapper;
    }
    
    public async Task<List<EntityRecordDto>> Handle(GetEntityRecordQuery request, CancellationToken cancellationToken)
    {
        var result = await _entityRecordService.GetAllAsync();
        return _mapper.Map<List<EntityRecordDto>>(result);
    }

}