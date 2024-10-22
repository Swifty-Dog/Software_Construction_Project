using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]

public class InventoriesController : ControllerBase{
    private readonly InventoriesServices _inventories;

    public InventoriesController(InventoriesServices inventories){
        _inventories = inventories;
    }

    [HttpGet("Inventories")]
    public async Task<IActionResult> Get_Inventories(){
        var inventories = await _inventories.Get_Inventories();
        return Ok(inventories);
    }

    [HttpGet("Inventory/{id}")]
    public async Task<IActionResult> Get_Inventory_By_Id(int id){
        var inventory = await _inventories.Get_Inventory_By_Id(id);
        if(inventory == null){
            return NotFound("No Inventory found with that ID");
        }
        return Ok(inventory);
    }

    [HttpGet("Inventory/{id}/locations")]
    public async Task<IActionResult> Get_Inventory_Locations(int id){
        var locations = await _inventories.Get_Inventory_Locations(id);
        if(locations == null){
            return NotFound("No locations found for that Inventory ID");
        }
        return Ok(locations);
    }

    [HttpPost("Inventory")]
   public async Task<IActionResult> AddInventory([FromBody] Inventory inventory)
    {
        if(inventory == null)
        {
            return BadRequest("Inventory is null");
        }
        // Call the service to add the inventory
        var addedInventory = await _inventories.Add_Inventory(inventory);

        if (addedInventory != null)
        {
            // If the inventory was successfully added, return success
            return Ok("Inventory and locations added successfully");
        }

        // If the inventory already exists, return an appropriate message
        return BadRequest("Inventory with the same id already exists.");
    }
    
    [HttpPut("Inventory/{id}")]
    public async Task<IActionResult> Update_Inventory(int id, [FromBody] Inventory inventory){
        var result = await _inventories.Update_Inventory(id, inventory);
        if(id <= 0 || id != inventory.Id){
            return BadRequest("Inventory ID is invalid or does not match the inventory ID in the request body.");
        }
        if(result == null){
            return BadRequest("Inventory could not be updated or does not exist.");
        }
        return Ok(result);
    }

    [HttpDelete("Inventory/{id}")]
    public async Task<IActionResult> Delete_Inventory(int id){
        var result = await _inventories.Delete_Inventory(id);
        if(result == false){
            return BadRequest("Inventory could not be deleted or does not exist.");
        }
        return Ok("Inventory deleted successfully");
    }
}