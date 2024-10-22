public interface IShipments{
    public Task<IEnumerable<Shipment>> Get_Shipments();
    public Task<Shipment> Get_Shipment_By_Id(int id);
    public Task<List<Shipments_item>> Get_Shipment_Items(int id);
    public Task<Shipment> Add_Shipment(Shipment shipment);
    public Task<Shipment> Update_Shipment(int id, Shipment shipment);
    public Task<bool> Delete_Shipment(int id);
}