using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("/api/v1/")]
public class WarehouseController : ControllerBase
{
    private readonly WarehouseServices _warehouse;

    
    public WarehouseController(WarehouseServices warehouse)
    {
        _warehouse = warehouse;
    }

    // GET /Warehouse: Returns all Warehouse. 
    [HttpGet("Warehouses")]
    public async Task<IActionResult> Get_Warehouses()
    {
        var warehouses = await _warehouse.Get_Warehouses(); 
        return Ok(warehouses); 
    }

    // GET /Warehouse/{id}: Returns the details of a specific warehouse by its ID. 
    [HttpGet("Warehouse/{id}")]
    public async Task<IActionResult> Get_Warehouse_By_Id(int id){    
        var warehouse = await _warehouse.Get_Warehouse_By_Id(id);
        if(warehouse == null){
            return NotFound("No Warehouse found with that ID");
        }
        return Ok(warehouse);
    }

    // GET /Warehouse/{id}/locations: Returns all locations within a specific warehouse. 
    // Needs to wait untill locations is made 

    [HttpGet("Warehouse/{id}/locations")]
    public async Task<IActionResult> Get_Warehouse_Locations(int id){
        if(id <= 0){
            return BadRequest("Invalid Warehouse ID");
        }
        var locations = await _warehouse.Get_Warehouse_LocationsAsync(id);
        if(locations == null){
            return NotFound("No locations found for that Warehouse ID");
        }
        return Ok(locations);
    }
    
    [HttpPost("Warehouse")]
    public async Task<IActionResult> Add_Warehouse([FromBody] Warehouse warehouse)
    {
        try{
            var result = await _warehouse.Add_Warehouse(warehouse);
            if (result == null){
                return BadRequest("Warehouse could not be added or already exists.");
            }
            return Ok(result);
        }
        catch (Exception ex){
            return BadRequest(ex.Message);  // Return the error message in a bad request response
        }
    }

    // PUT /Warehouse/{id}: Updates warehouse information. 
    [HttpPut("Warehouse/{id}")]
    public async Task<IActionResult> Update_Warehouse([FromRoute]int id, [FromBody] Warehouse warehouse){
        try{
            if(id <= 0 || id != warehouse.Id){
                return BadRequest("Warehouse ID is invalid or does not match the warehouse ID in the request body.");
            }
            var result = await _warehouse.Update_Warehouse(id, warehouse);
            if (result == null) {
                return BadRequest("Warehouse could not be updated.");
            }
            // if(id >=0)
            //     return BadRequest("ID can not be a negative number");
            return Ok(result);
        }
        catch (Exception ex){
            return BadRequest(ex.Message);  // Return the error message in a bad request response
        }
    }


    // DELETE /Warehouse/ {id}: Deletes a warehouse. 
    [HttpDelete("Warehouse/{id}")]
    public async Task<bool> Delete_Warehouse(int id){
        bool WarehouseToDeleted = await _warehouse.Delete_Warehouse(id);
        if(WarehouseToDeleted == false)
            return false;
        return true;
    }
}



