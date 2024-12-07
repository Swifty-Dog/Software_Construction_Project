public interface IShipments
{
    public Task<IEnumerable<Shipment>> GetShipments();
    public Task<Shipment> GetShipmentById(int id);
    public Task<List<ShipmentsItem>> GetShipmentItems(int id);
    public Task<Shipment> AddShipment(Shipment shipment);
    public Task<Shipment> UpdateShipment(int id, Shipment shipment);
    public Task<bool> DeleteShipment(int id);
}