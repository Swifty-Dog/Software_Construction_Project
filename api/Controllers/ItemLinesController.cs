using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/")]
public class ItemLinesController : ControllerBase{
    private readonly ItemLineServices _itemLine;
    public ItemLinesController(ItemLineServices itemLine)
    {
        _itemLine = itemLine;
    }

    [HttpGet("ItemLines")]
    public async Task<IActionResult> GetItemLines()
    {
        var itemLines = await _itemLine.GetItemLine();
        return Ok(itemLines);
    }

    [HttpGet("ItemLines/{id}")]
    public async Task<IActionResult> GetItemLineById(int id)
    {
        var itemLine = await _itemLine.GetItemLineById(id);
        if (itemLine == null)
        {
            return NotFound("No Item Line found with that ID");
        }
        return Ok(itemLine);
    }

    [HttpPost("ItemLine")]
    public async Task<IActionResult> AddItemLine([FromBody] ItemLine itemLine)
    {
        if (itemLine == null)
            return BadRequest("Item Line is null.");
        var result = await _itemLine.AddItemLine(itemLine);
        if (result == null)
            return BadRequest("Item Line already exists.");
        return Ok(result);
    }

    [HttpPut("ItemLine/{id}")]
    public async Task<IActionResult> UpdateItemLine([FromRoute] int id, [FromBody] ItemLine itemLine)
    {
        if(id <= 0 || id != itemLine.Id)
            return BadRequest("Item Line ID in the body does not match the ID in the URL.");
        if (itemLine == null)
            return BadRequest("Item Line is null.");
        var result = await _itemLine.UpdateItemLine(itemLine, id);
        if (result == null)
            return NotFound("Item Line not found or has been deleted.");        
        return Ok(result);
    }

    [HttpDelete("ItemLine/{id}")]
    public async Task<IActionResult> DeleteItemLine([FromRoute] int id)
    {
        if(id <= 0)
            return NotFound();
        var result = await _itemLine.DeleteItemLine(id);
        if (result == false)
            return NotFound();
        return NoContent();
    }
    
}