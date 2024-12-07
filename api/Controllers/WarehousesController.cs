using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]
public class WarehouseController : ControllerBase
{
    private readonly WarehouseServices _warehouse;

    public WarehouseController(WarehouseServices warehouse)
    {
        _warehouse = warehouse;
    }

    // GET /Warehouses: Returns all Warehouses.
    [HttpGet("Warehouses")]
    public async Task<IActionResult> GetWarehouses()
    {
        try
        {
            var warehouses = await _warehouse.GetWarehouses();
            if (warehouses == null || !warehouses.Any())
            {
                return NotFound("No Warehouses found.");
            }
            return Ok(warehouses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET /Warehouse/{id}: Returns the details of a specific warehouse by its ID.
    [HttpGet("Warehouse/{id}")]
    public async Task<IActionResult> GetWarehouseById(int id)
    {
        try
        {
            var warehouse = await _warehouse.GetWarehouseById(id);
            if (warehouse == null)
            {
                return NotFound($"No Warehouse found with ID {id}.");
            }
            return Ok(warehouse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET /Warehouse/{id}/locations: Returns all locations within a specific warehouse.
    [HttpGet("Warehouse/{id}/locations")]
    public async Task<IActionResult> GetWarehouseLocations(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid Warehouse ID.");
        }
        try
        {
            var locations = await _warehouse.GetWarehouseLocations(id);
            if (locations == null || !locations.Any())
            {
                return NotFound($"No locations found for Warehouse ID {id}.");
            }
            return Ok(locations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST /Warehouse: Adds a new warehouse.
    [HttpPost("Warehouse")]
    public async Task<IActionResult> AddWarehouse([FromBody] Warehouse warehouse)
    {
        try
        {
            var result = await _warehouse.AddWarehouse(warehouse);
            if (result == null)
            {
                return BadRequest("Warehouse could not be added or already exists.");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);  // Return the error message in a bad request response
        }
    }


    // PUT /Warehouse/{id}: Updates warehouse information.
    [HttpPut("Warehouse/{id}")]
    public async Task<IActionResult> UpdateWarehouse(int id, [FromBody] Warehouse warehouse)
    {
        if (id <= 0 || id != warehouse.Id)
        {
            return BadRequest("Warehouse ID is invalid or does not match the request body.");
        }
        try
        {
            var result = await _warehouse.UpdateWarehouse(id, warehouse);
            if (result == null)
            {
                return NotFound($"No Warehouse found with ID {id}.");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE /Warehouse/{id}: Deletes a warehouse.
    [HttpDelete("Warehouse/{id}")]
    public async Task<IActionResult> DeleteWarehouse(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid Warehouse ID.");
        }
        try
        {
            var deleted = await _warehouse.DeleteWarehouse(id);
            if (!deleted)
            {
                return NotFound($"No Warehouse found with ID {id}.");
            }
            return NoContent(); // 204 No Content
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
