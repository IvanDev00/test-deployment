using riskwatch.api.search.Entities;

namespace riskwatch.api.search.Features.Common.Dto;

public class EntityRecordDto
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? BirthDate { get; set; }
    public List<Aliases>? Aliases { get; set; }
    public List<Address>? AddressDetails { get; set; }
    public List<Contact>? ContactDetails { get; set; }
    public string? RiskwatchNumber { get; set; }
}