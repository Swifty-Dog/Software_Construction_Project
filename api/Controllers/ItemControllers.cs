using Microsoft.AspNetCore.Mvc;
using Serilog;
using Newtonsoft.Json;

[ApiController]
[Route("/api/v1/")]
public class ItemController : Controller
{
    private readonly ItemServices _item;
    private readonly ILogger<ItemController> _logger;

    public ItemController(ItemServices item, ILogger<ItemController>? logger = null)
    {
        _item = item;
        _logger = logger;
    }

    [HttpGet("Items")]
    public async Task<IActionResult> GetItems()
    {
        _logger?.LogInformation("GET /api/v1/Item: Retrieved all items.");
        var items = await _item.GetItems();
        return Ok(items);
    }

    [HttpGet("Item/{uid}")]
    public async Task<IActionResult> GetItemById(string uid)
    {
        var item = await _item.GetItemById(uid);
        if (item == null)
        {
            _logger?.LogInformation("GET /api/v1/Item: Item with id {uid} not found.",uid);
            return NotFound("No Item found with that ID");
        }
        _logger?.LogInformation("GET /api/v1/Item/{Id}: Item with ID {Id} retrieved.", uid);
        return Ok(item);
    }

    [HttpPost("Item")]
    public async Task<IActionResult> AddItem([FromBody] Item item)
    {
        try
        {
            var result = await _item.AddItem(item);
            if (result == null)
            {
                _logger?.LogInformation("POST /api/v1/Item: Item could not be added or already exists.");
                return BadRequest("Item could not be added or already exists.");
            }
            //_logger.LogInformation("POST /api/v1/Item: Item with ID {Uid} added successfully", item.Uid);
            _logger?.LogInformation("POST /api/v1/Item: Item added successfully. Details: {@Item}", item);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogInformation("POST /api/v1/Item: Item could not be added or already exists.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Item/{uid}")]
    public async Task<IActionResult> UpdateItem([FromRoute] string uid, [FromBody] Item item)
    {
        var oldItem = await _item.GetItemById(uid);
         var oldItemSnapshot = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(oldItem));

        try
        {
            if (string.IsNullOrEmpty(uid))
            {
                _logger?.LogInformation("PUT /api/v1/Item: Item with id: {uid} could not be updated.", uid);
                return BadRequest("Item ID is invalid or does not match the item ID in the request body.");
            }

            var result = await _item.UpdateItem(uid, item);
            if (result == null)
            {
                _logger?.LogInformation("PUT /api/v1/Item: Item with id: {uid} could not be updated.", uid);
                return BadRequest("Item could not be updated.");
            }
            _logger?.LogInformation("PUT /api/v1/Item/{Uid}: Item updated. Old Item: {@OldItem}, New Item: {@UpdatedItem}",uid, oldItemSnapshot, result);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogInformation("PUT /api/v1/Item: Item with id: {uid} could not be updated.", uid);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("Item/{uid}")]
    public async Task<IActionResult> DeleteItem(string uid)
    {
        try
        {
            bool itemToDeleted = await _item.DeleteItem(uid);
            if (itemToDeleted == false)
            {
                return BadRequest("Item could not be deleted.");
            }
            _logger?.LogInformation("DELETE /api/v1/Item/{Uid}: Item with ID {Uid} deleted successfully", uid,uid);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Item/{uid}/Inventory")]
    public async Task<IActionResult> GetIventoryThroughItems(string uid)
    {
        var item = await _item.GetIventoryThroughItems(uid);
        if (item == null)
        {
            return NotFound("No Item found with that ID");
        }
        return Ok(item);
    }

    [HttpGet("Item/{uid}/Inventory/Totals")]
    public async Task<IActionResult> GetItemTotalsFromInventory(string uid)
    {
        var item = await _item.GetItemTotalsFromInventory(uid);
        if (item == null)
        {
            return NotFound("No Item found with that ID");
        }
        return Ok(item);
    }
}
