using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]

public class ItemTypeController : ControllerBase{
    private readonly Item_TypeServices _item_types;
    public ItemTypeController(Item_TypeServices item_types){
        _item_types = item_types;
    }

    [HttpGet("ItemTypes")]
    public async Task<IActionResult> GetItem_types(){
        var item_types = await _item_types.GetItem_types();
        return Ok(item_types);
    }

    [HttpGet("ItemType/{id}")]
    public async Task<IActionResult> GetItem_types_By_Id(int id){
        var item_types = await _item_types.GetItem_types_By_Id(id);
        if (item_types == null){
            return NotFound("No Item Type found with that ID");
        }
        return Ok(item_types);
    }
    
    [HttpPut("ItemType/{id}")]
    public async Task<IActionResult> UpdateItem_types([FromRoute] int id, [FromBody] Item_type item_types){
        if(id <= 0 || id != item_types.Id)
            return BadRequest("Item Type ID in the body does not match the ID in the URL.");
        if (item_types == null)
            return BadRequest("Item Type is null.");
        var result = await _item_types.UpdateItem_types(item_types);
        if (result == null)
            return NotFound("Item Type not found or has been deleted.");        
        return Ok(result);
    }

    [HttpDelete("ItemType/{id}")]
    public async Task<bool> DeleteItem_types([FromRoute] int id){
        if(id <= 0)
            return false;
        var result = await _item_types.DeleteItem_types(id);
        if (result == false)
            return false;
        return true;
    }
}