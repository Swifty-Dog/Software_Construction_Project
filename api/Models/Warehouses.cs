public class Warehouses{
    public required int Id { get; set; }
    public required string code { get; set; }
    public required string name { get; set; }
    public required string address { get; set; }
    public required string zip { get; set; }
    public required string city { get; set; }
    public required string country { get; set; }
    public required Contact contact {get; set; }
    public required DateTime created_at { get; set; }

}
