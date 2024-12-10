using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]
public class ItemGroupController : ControllerBase
{
    private readonly ItemGroupService _itemGroupService;
    public ItemGroupController(ItemGroupService itemGroup)
    {
        _itemGroupService = itemGroup;
    }

    [HttpGet("ItemGroups")]
    public async Task<IActionResult> GetItemGroups()
    {
        var itemGroups = await _itemGroupService.GetItemGroups();
        return Ok(itemGroups);
    }

    [HttpGet("ItemGroups/{id}")]
    public async Task<IActionResult> GetItemGroupById(int id)
    {
        var itemGroup = await _itemGroupService.GetItemGroupById(id);
        if (itemGroup == null)
        {
            return NotFound("No Item Group found with that ID");
        }
        return Ok(itemGroup);
    }
    
    [HttpPost("ItemGroup")]
    public async Task<IActionResult> Add_Item_group([FromBody] ItemGroup itemGroup)
    {
        if (itemGroup == null)
            return BadRequest("Item Group is null.");
        var result = await _itemGroupService.AddItemGroup(itemGroup);
        if (result != null)
            return Ok(result);
        return BadRequest("Item Group already exists.");
        
    }

    [HttpPut("ItemGroups/{id}")]
    public async Task<IActionResult> UpdateItemGroup([FromRoute] int id, [FromBody] ItemGroup itemGroup)
    {
        if(id <= 0 || id != itemGroup.Id)
            return BadRequest("Item Group ID in the body does not match the ID in the URL.");
        if (itemGroup == null)
            return BadRequest("Item Group is null.");
        var result = await _itemGroupService.UpdateItemGroup(itemGroup);
        if (result == null)
            return NotFound("Item Group not found or has been deleted.");        
        return Ok(result);
    }

    [HttpDelete("ItemGroup/{id}")]
    public async Task<IActionResult> DeleteItemGroup([FromRoute] int id)
    {
        if(id <= 0)
            return BadRequest();
        var result = await _itemGroupService.DeleteItemGroup(id);
        if (result == false)
            return BadRequest();
        return NoContent();
    }
}