namespace riskwatch.api.search.Entities;
public class Address
{
    public string? Region { get; set; }
    public string? City { get; set; }
    public string? ZipCode { get; set; }
    public string? Province { get; set; }
    public string? Purok { get; set; }
    public string? Subdivision { get; set; }
    public string? Country { get; set; }
    public string? StreetName { get; set; }
    public string? UnitNumber { get; set; }
    public string? BuildingNumbwer { get; set; }
    public string? HouseNumber { get; set; }
    public string? OtherInformation { get; set; }
    public string? PermanentAddress { get; set; }
    public string? PresentAddress { get; set; }
    public string? Municipality { get; set; } = null!;
    public string? Baranggay { get; set; } = null!;


}
