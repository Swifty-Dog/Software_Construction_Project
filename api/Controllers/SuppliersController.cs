using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("/api/v1/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly ISuppliersInterface _suppliersServices;

    public SuppliersController(ISuppliersInterface suppliersServices)
    {
        _suppliersServices = suppliersServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers()
    {
        var suppliers = await _suppliersServices.GetAll();
        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplier(int id)
    {
        var supplier = await _suppliersServices.Get(id);
        if (supplier == null)
        {
            return NotFound("Supplier not found.");
        }
        return Ok(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> AddSupplier([FromBody] Supplier supplier)
    {
        try
        {
            var result = await _suppliersServices.AddSupplier(supplier);
            if (result == null)
            {
                return BadRequest("Supplier could not be added or already exists.");
            }
            return CreatedAtAction(nameof(GetSupplier), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] Supplier supplier)
    {
        try
        {
            var result = await _suppliersServices.UpdateSupplier(id, supplier);
            if (result == null)
            {
                return NotFound("Supplier not found or could not be updated.");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        try
        {
            var deleted = await _suppliersServices.DeleteSupplier(id);
            if (!deleted)
            {
                return NotFound("Supplier not found or could not be deleted.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
