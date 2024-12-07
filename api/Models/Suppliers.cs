public class Supplier
{
    public required int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Address_extra { get; set; }
    public required string Zip_code { get; set; }
    public required string Province { get; set; }
    public required string Country { get; set; }
    public required string Contact_name {get; set;}
    public required string Phonenumber {get; set;}
    public required string Email {get; set;}
    public required string Reference {get; set; }
    public required DateTime Created_at { get; set; }
    public required DateTime Updated_at { get; set; }
}
