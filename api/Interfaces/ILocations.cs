public interface ILocations
{
    Task<Locations> GetLocationById(int id);
    Task<IEnumerable<Locations>> GetLocations();
    Task<Locations> AddLocation(Locations location);
    Task<Locations> UpdateLocation(int id, Locations location);
    Task<bool> DeleteLocation(int id);
}
