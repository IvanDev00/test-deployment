using riskwatch.api.search.Features.Common.Dto;
using riskwatch.api.search.Features.FuzzySearch.Dto;

namespace riskwatch.api.search.Features.FuzzySearch.Service.EntityRecordService;

public interface IEntityRecordService
{
    Task<List<EntityRecordDto>> GetAllAsync();
    Task<List<SearchResultDto>> GetAndMapEntityRecordsAsync();
}