using MediatR;
using riskwatch.api.search.Features.Common.Dto;

namespace riskwatch.api.search.Features.EntityRecords.Queries.GetAllEntityRecord;

public class GetEntityRecordQuery : IRequest<List<EntityRecordDto>>
{
}