public class EndpointAccess
{
    public int Id { get; set; } // Primary key
    public string? Endpoint { get; set; } // e.g., "warehouses", "locations"
    public bool CanGet { get; set; }
    public bool CanPost { get; set; }
    public bool CanPut { get; set; }
    public bool CanDelete { get; set; }

    public int UserId { get; set; } // Foreign key to User
    public User? User { get; set; }
}
