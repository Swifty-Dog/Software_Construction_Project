using Microsoft.AspNetCore.Mvc;
using Serilog;

[ApiController]
[Route("/api/v1/")]
public class ItemController : Controller
{
    private readonly ItemServices _item;
    private readonly ILogger<ItemController> _logger;

    public ItemController(ItemServices item, ILogger<ItemController> logger)
    {
        _item = item;
        _logger = logger;
    }

    [HttpGet("Items")]
    public async Task<IActionResult> GetItems()
    {
        var items = await _item.GetItems();
        return Ok(items);
    }

    [HttpGet("Item/{uid}")]
    public async Task<IActionResult> GetItemById(string uid)
    {
        var item = await _item.GetItemById(uid);
        if (item == null)
        {
            return NotFound("No Item found with that ID");
        }
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
                return BadRequest("Item could not be added or already exists.");
            }
            _logger.LogInformation("POST /api/v1/Item: Item with ID {Uid} added successfully", item.Uid);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Item/{uid}")]
    public async Task<IActionResult> UpdateItem([FromRoute] string uid, [FromBody] Item item)
    {
        try
        {
            if (string.IsNullOrEmpty(uid))
            {
                return BadRequest("Item ID is invalid or does not match the item ID in the request body.");
            }

            var result = await _item.UpdateItem(uid, item);
            if (result == null)
            {
                return BadRequest("Item could not be updated.");
            }
            _logger.LogInformation("PUT /api/v1/Item/{Uid}: Item with ID {Uid} updated successfully", uid, uid);
            return Ok(result);
        }
        catch (Exception ex)
        {
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
            _logger.LogInformation("DELETE /api/v1/Item/{Uid}: Item with ID {Uid} deleted successfully", uid,uid);
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
