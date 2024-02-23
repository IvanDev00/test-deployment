namespace riskwatch.api.search.Entities;

public class Entity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int EntityType { get; set; }
    public int? Carrier { get; set; }
    public int ReferenceId { get; set; }
    public string? CustomerNumber { get; set; }
    public string? OfacNumber { get; set; }
    public string? UnscNumber { get; set; }
    public string? UKNumber { get; set; }
    public string? EUNumber { get; set; }

    // Navigation Property 
    public Individual Individual { get; set; }
    public Corporate Corporate { get; set; }
}