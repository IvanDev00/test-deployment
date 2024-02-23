namespace riskwatch.api.search.Entities;

public class Birth
{
    public DateTime BirthdateEntry { get; set; }
    public string? BirthCountry { get; set; }
    public string? BirthRegion { get; set; }
    public string? BirthCity { get; set; }
    public string? BirthProvince { get; set; }
    public string? BirthBarangay { get; set; }
    public string? BirthZipCode { get; set; }
}