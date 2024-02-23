using Microsoft.EntityFrameworkCore;

namespace riskwatch.api.search.Entities;

[Keyless]
public class Sanction
{
    public string? UnscReferenceNumber { get; set; }
    public string? UKReferenceNumber { get; set; }
}