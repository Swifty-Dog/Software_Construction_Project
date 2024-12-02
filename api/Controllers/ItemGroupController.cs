using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]
public class ItemGroupController : ControllerBase{
    private readonly Item_groupService _item_group;
    public ItemGroupController(Item_groupService item_group){
        _item_group = item_group;
    }

    [HttpGet("ItemGroups")]
    public async Task<IActionResult> Get_Item_groups(){
        var item_groups = await _item_group.Get_Item_groups();
        return Ok(item_groups);
    }

    [HttpGet("ItemGroups/{id}")]
    public async Task<IActionResult> Get_Item_group_By_Id(int id){
        var item_group = await _item_group.Get_Item_group_By_Id(id);
        if (item_group == null){
            return NotFound("No Item Group found with that ID");
        }
        return Ok(item_group);
    }
    
    [HttpPost("ItemGroup")]
    public async Task<IActionResult> Add_Item_group([FromBody] Item_group item_group)
    {
        if (item_group == null)
            return BadRequest("Item Group is null.");
        var result = await _item_group.AddItemGroup(item_group);
        if (result != null)
            return Ok(result);
        return BadRequest("Item Group already exists.");
        
    }

    [HttpPut("ItemGroups/{id}")]
    public async Task<IActionResult> Update_Item_group([FromRoute] int id, [FromBody] Item_group item_group){
        if(id <= 0 || id != item_group.Id)
            return BadRequest("Item Group ID in the body does not match the ID in the URL.");
        if (item_group == null)
            return BadRequest("Item Group is null.");
        var result = await _item_group.Update_Item_group(item_group);
        if (result == null)
            return NotFound("Item Group not found or has been deleted.");        
        return Ok(result);
    }

    [HttpDelete("ItemGroup/{id}")]
    public async Task<IActionResult> Delete_Item_group([FromRoute] int id){
        if(id <= 0)
            return BadRequest();
        var result = await _item_group.Delete_Item_group(id);
        if (result == false)
            return BadRequest();
        return NoContent();
    }
}