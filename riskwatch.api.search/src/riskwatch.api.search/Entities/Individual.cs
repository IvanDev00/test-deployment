namespace riskwatch.api.search.Entities;

public class Individual
{
    public int Id { get; set; }
    public int? ImportsLogId { get; set; }
    public string? Fullname { get; set; }
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? BirthDate { get; set; } = null!;
    public string? MiddleName { get; set; } = null!;
    public string? Suffix { get; set; } = null!;
    public string? RiskwatchNumber { get; set; } = null!;
    public List<string> Title { get; set; } = null!;
    public string? Nickname { get; set; } = null!;
    public int? EntityId { get; set; } = null!;
    public Sanction SanctionDetails { get; set; } = new Sanction();
    public List<Birth> BirthDetails { get; set; } = null!;
    public List<Personal> PersonalDetails { get; set; } = null!;
    public List<Other> OtherDetails { get; set; } = null!;
    public List<Record> RecordDetails { get; set; } = null!;
    public List<Address> AddressDetails { get; set; } = null!;
    public List<Aliases> Aliases { get; set; } = null!;
    public List<Contact> ContactDetails { get; set; } = null!;
    public List<Financial> FinancialDetails { get; set; } = null!;
    public List<IdDetails> IdDetails { get; set; } = null!;
    public List<Document> DocumentDetails { get; set; } = null!;
}