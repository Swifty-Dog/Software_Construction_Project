using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]

public class ItemTypeController : ControllerBase{
    private readonly ItemTypeServices _itemTypesService;
    public ItemTypeController(ItemTypeServices itemTypes)
    {
        _itemTypesService = itemTypes;
    }

    [HttpGet("ItemTypes")]
    public async Task<IActionResult> GetItemTypes()
    {
        var itemTypes = await _itemTypesService.GetItemTypes();
        return Ok(itemTypes);
    }

    [HttpGet("ItemTypes/{id}")]
    public async Task<IActionResult> GetItemTypesById(int id)
    {
        var itemTypes = await _itemTypesService.GetItemTypesById(id);
        if (itemTypes == null)
        {
            return NotFound("No Item Type found with that ID");
        }
        return Ok(itemTypes);
    }
    
    [HttpPost("ItemType")]
    public async Task<IActionResult> AddItemType([FromBody] ItemType itemTypes)
    {
        if (itemTypes == null)
            return BadRequest("Item Type is null.");
        var result = await _itemTypesService.AddItemType(itemTypes);
        if (result == null)
            return BadRequest("Item Type already exists.");
        return Ok(result);
    }

    [HttpPut("ItemType/{id}")]
    public async Task<IActionResult> UpdateItemTypes([FromRoute] int id, [FromBody] ItemType itemTypes)
    {
        if(id <= 0 || id != itemTypes.Id)
            return BadRequest("Item Type ID in the body does not match the ID in the URL.");
        if (itemTypes == null)
            return BadRequest("Item Type is null.");
        var result = await _itemTypesService.UpdateItemTypes(itemTypes);
        if (result == null)
            return NotFound("Item Type not found or has been deleted.");        
        return Ok(result);
    }

    [HttpDelete("ItemType/{id}")]
    public async Task<IActionResult> DeleteItemTypes([FromRoute] int id)
    {
        if(id <= 0)
            return NotFound();
        var result = await _itemTypesService.DeleteItemTypes(id);
        if (result == false)
            return NotFound();
        return NoContent();
    }
}