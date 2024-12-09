using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]
public class ItemController : Controller{
    private readonly ItemServices _item;
    public ItemController(ItemServices item)
    {
        _item = item;
    }

    // GET /Items: Returns all Items. 
    [HttpGet("Items")]
    public async Task<IActionResult> GetItems()
    {
        var items = await _item.GetItems(); 
        return Ok(items); 
    }

    // GET /Item/{uid}: Returns the details of a specific item by its ID. 
    [HttpGet("Item/{uid}")]
    public async Task<IActionResult> GetItemById(string uid){    
        var item = await _item.GetItemById(uid);
        if(item == null){
            return NotFound("No Item found with that ID");
        }
        return Ok(item);
    }
    
    [HttpPost("Item")]
    public async Task<IActionResult> AddItem([FromBody] Item item)
    {
        try{
            var result = await _item.AddItem(item);
            if (result == null){
                return BadRequest("Item could not be added or already exists.");
            }
            return Ok(result);
        }
        catch (Exception ex){
            return BadRequest(ex.Message);  // Return the error message in a bad request response
        }
    }

    // PUT /Item/{uid}: Updates item information. 
    [HttpPut("Item/{uid}")]
    public async Task<IActionResult> UpdateItem([FromRoute]string uid, [FromBody] Item item){
        try{
            //if(id <= 0 || uid != item.Uid){
              //  return BadRequest("Item ID is invalid or does not match the item ID in the request body.");
            if (uid == null || uid.Length == 0){
                return BadRequest("Item ID is invalid or does not match the item ID in the request body.");
            }
            
            var result = await _item.UpdateItem(uid, item);
            if (result == null) {
                return BadRequest("Item could not be updated.");
            }
            return Ok(result);
        }
        catch (Exception ex){
            return BadRequest(ex.Message);  // Return the error message in a bad request response
        }
    }

    // DELETE /Item/ {uid}: Deletes a client. 
    [HttpDelete("Item/{uid}")]
    public async Task<IActionResult> DeleteItem(string uid){
        bool itemToDeleted = await _item.DeleteItem(uid);
        if(itemToDeleted == false)
            return BadRequest("Item could not be deleted.");
        return NoContent();
    }

    // GET /Item/{uid}/Inventory: Returns the inventory of a specific item by its ID.
    [HttpGet("Item/{uid}/Inventory")]
    public async Task<IActionResult> GetIventoryThroughItems(string uid){
        var item = await _item.GetIventoryThroughItems(uid);
        if(item == null){
            return NotFound("No Item found with that ID");
        }
        return Ok(item);
    }

    // GET /Item/{uid}/Totals: Returns the total of a specific item by its ID.  // from inventory only the total expected tot available
    [HttpGet("Item/{uid}/Inventory/Totals")]
    public async Task<IActionResult> GetItemTotalsFromInventory(string uid){
        var item = await _item.GetItemTotalsFromInventory(uid);
        if(item == null){
            return NotFound("No Item found with that ID");
        }
        return Ok(item);
    }
}