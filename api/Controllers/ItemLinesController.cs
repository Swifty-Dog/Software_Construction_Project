using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]
public class ItemLinesController : ControllerBase{
    private readonly Item_lineServices _item_line;
    public ItemLinesController(Item_lineServices item_line){
        _item_line = item_line;
    }

    [HttpGet("ItemLines")]
    public async Task<IActionResult> Get_Item_groups(){
        var item_groups = await _item_line.Get_Item_groups();
        return Ok(item_groups);
    }

    [HttpGet("ItemLine/{id}")]
    public async Task<IActionResult> Get_Item_group_By_Id(int id){
        var item_group = await _item_line.Get_Item_group_By_Id(id);
        if (item_group == null){
            return NotFound("No Item Group found with that ID");
        }
        return Ok(item_group);
    }

    [HttpPut("ItemLine/{id}")]
    public async Task<IActionResult> Update_Item_group([FromRoute] int id, [FromBody] Item_line item_group){
        if(id <= 0 || id != item_group.Id)
            return BadRequest("Item Group ID in the body does not match the ID in the URL.");
        if (item_group == null)
            return BadRequest("Item Group is null.");
        var result = await _item_line.Update_Item_group(item_group);
        if (result == null)
            return NotFound("Item Group not found or has been deleted.");        
        return Ok(result);
    }

    [HttpDelete("ItemLine/{id}")]
    public async Task<bool> Delete_Item_group([FromRoute] int id){
        if(id <= 0)
            return false;
        var result = await _item_line.Delete_Item_group(id);
        if (result == false)
            return false;
        return true;
    }
    
}