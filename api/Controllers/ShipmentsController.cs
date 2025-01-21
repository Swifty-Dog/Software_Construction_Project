using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

[ApiController]
[Route("/api/v1/")]
public class ShipmentsController : ControllerBase
{
    private readonly ShipmentsServices _shipments;
    private readonly ILogger<ShipmentsController> _logger;

    public ShipmentsController(ShipmentsServices shipments, ILogger<ShipmentsController>? logger = null)
    {
        _shipments = shipments;
        _logger = logger;
    }

    [HttpGet("Shipments")]
    public async Task<IActionResult> GetShipments()
    {
        var shipments = await _shipments.GetShipments();
        _logger?.LogInformation("GET /api/v1/Shipments: Retrieved all shipments.");
        return Ok(shipments);
    }

    [HttpGet("Shipment/{id}")]
    public async Task<IActionResult> GetShipmentById(int id)
    {
        var shipment = await _shipments.GetShipmentById(id);
        if (shipment == null)
        {
            _logger?.LogInformation("GET /api/v1/Shipment/{Id}: Shipment with ID {Id} not found.", id);
            return NotFound("No Shipment found with that ID");
        }
        _logger?.LogInformation("GET /api/v1/Shipment/{Id}: Shipment retrieved successfully. Details: {@Shipment}", id, shipment);
        return Ok(shipment);
    }

    [HttpGet("Shipment/{id}/items")]
    public async Task<IActionResult> GetShipmentItems(int id)
    {
        var items = await _shipments.GetShipmentItems(id);
        if (items == null)
        {
            _logger?.LogInformation("GET /api/v1/Shipment/{Id}/items: No items found for Shipment ID {Id}.", id);
            return NotFound("No items found for that Shipment ID");
        }
        _logger?.LogInformation("GET /api/v1/Shipment/{Id}/items: Retrieved items for Shipment ID {Id}. Details: {@Items}", id, items);
        return Ok(items);
    }

    [HttpPost("Shipment")]
    public async Task<IActionResult> AddShipment([FromBody] Shipment shipment)
    {
        if (shipment == null)
        {
            _logger?.LogInformation("POST /api/v1/Shipment: Shipment is null.");
            return BadRequest("Shipment is null");
        }

        var addedShipment = await _shipments.AddShipment(shipment);
        if (addedShipment != null)
        {
            _logger?.LogInformation("POST /api/v1/Shipment: Shipment added successfully. Details: {@Shipment}", shipment);
            return Ok("Shipment and items added successfully");
        }

        _logger?.LogInformation("POST /api/v1/Shipment: Shipment with ID {Id} already exists.", shipment.Id);
        return BadRequest("Shipment with the same id already exists.");
    }

    [HttpPut("Shipment/{id}")]
    public async Task<IActionResult> UpdateShipment(int id, [FromBody] Shipment shipment)
    {
        var oldShipment = await _shipments.GetShipmentById(id);
        var oldShipmentSnapshot = JsonConvert.DeserializeObject<Shipment>(JsonConvert.SerializeObject(oldShipment));

        var result = await _shipments.UpdateShipment(id, shipment);
        if (id <= 0 || id != shipment.Id)
        {
            _logger?.LogInformation("PUT /api/v1/Shipment/{Id}: Invalid or mismatched ID in request body.", id);
            return BadRequest("Shipment ID is invalid or does not match the shipment ID in the request body.");
        }
        if (result == null)
        {
            _logger?.LogInformation("PUT /api/v1/Shipment/{Id}: Shipment with ID {Id} could not be updated.", id);
            return BadRequest("Shipment could not be updated or does not exist.");
        }

        _logger?.LogInformation("PUT /api/v1/Shipment/{Id}: Shipment updated. Old Shipment: {@OldShipment}, Updated Shipment: {@UpdatedShipment}", id, oldShipmentSnapshot, result);
        return Ok(result);
    }

    [HttpDelete("Shipment/{id}")]
    public async Task<IActionResult> DeleteShipment(int id)
    {
        var shipmentToDelete = await _shipments.GetShipmentById(id);

        if (shipmentToDelete == null)
        {
            _logger?.LogInformation("DELETE /api/v1/Shipment/{Id}: Shipment with ID {Id} could not be found before deletion.", id);
            return NotFound("Shipment not found or already deleted.");
        }

        var shipmentSnapshot = JsonConvert.SerializeObject(shipmentToDelete);
        _logger?.LogInformation("DELETE /api/v1/Shipment/{Id}: Preparing to delete shipment. Details: {@ShipmentSnapshot}", id, shipmentSnapshot);

        var result = await _shipments.DeleteShipment(id);
        if (!result)
        {
            _logger?.LogInformation("DELETE /api/v1/Shipment/{Id}: Shipment with ID {Id} could not be deleted or does not exist.", id);
            return NotFound("Shipment not found or already deleted.");
        }

        _logger?.LogInformation("DELETE /api/v1/Shipment/{Id}: Shipment with ID {Id} deleted successfully. Details: {@ShipmentSnapshot}", id, shipmentSnapshot);
        return Ok("Shipment deleted successfully");
    }

}
