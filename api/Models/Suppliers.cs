public class Supplier
{
    public required int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string AddressExtra { get; set; }
    public required string City {get; set;}
    public required string ZipCode { get; set; }
    public required string Province { get; set; }
    public required string Country { get; set; }
    public required string ContactName {get; set;}
    public required string Phonenumber {get; set;}
    public required string Reference {get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}
