public class Warehouse{
    public required int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Zip { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required Contact Contact {get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
    public ICollection<Locations> Locations { get; set; } = new List<Locations>();


}