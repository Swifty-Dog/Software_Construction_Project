public interface ILocationinterface
{
    Task<Locations> Get(int id);
    Task<IEnumerable<Locations>> GetAll();
    Task<Locations> Add_Location(Locations location);
    Task<Locations> Update_Location(int id, Locations location);
    Task<bool> Delete_Location(int id);
}
