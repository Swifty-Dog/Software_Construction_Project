public class User
{
    public int Id { get; set; } // Primary key
    public required string ApiKey { get; set; } // The user's API key
    public required string AppName { get; set; } // (Optional) The name of the app associated with the key
    public bool HasFullAccess { get; set; } // (Optional) If the user has full access to all endpoints
    public List<EndpointAccess> EndpointAccesses { get; set; } = new List<EndpointAccess>(); // List of specific permissions if full access is false

}
