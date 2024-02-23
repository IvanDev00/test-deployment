using Nest;
using riskwatch.api.search.Features.Common.Dto;
using riskwatch.api.search.Features.FuzzySearch.Dto;
using Profile = AutoMapper.Profile;

namespace APIBoilerplate.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EntityRecordDto, SearchResultDto>()
            .ForMember(dest => dest.Suggest, opt =>
                opt.MapFrom(src => new CompletionField
                {
                    Input = new List<string> { src.Name }
                        .Where(input => !string.IsNullOrWhiteSpace(input))
                        .ToList()
                }));
    }
}