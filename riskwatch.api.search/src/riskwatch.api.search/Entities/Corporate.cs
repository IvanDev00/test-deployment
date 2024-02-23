using System.Text.Json;

namespace riskwatch.api.search.Entities;

public class Corporate
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string RiskwatchNumber { get; set; } = null!;
    public int? ImportsLogId { get; set; }
    public int? EntityId { get; set; }
    public Sanction SanctionDetails { get; set; } = new Sanction();

    public List<Associated>? AssociatedEntities { get; set; }
    public List<Other> OtherDetails { get; set; } = null!;
    public List<Record> RecordDetails { get; set; } = null!;
    public List<Address> AddressDetails { get; set; } = null!;
    public List<Aliases> Aliases { get; set; } = null!;
    public List<Contact> ContactDetails { get; set; } = null!;
    public List<IdDetails> IdDetails { get; set; } = null!;
   
}

