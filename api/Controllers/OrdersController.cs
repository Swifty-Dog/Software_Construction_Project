using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("/api/v1/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersInterface _ordersServices;

    public OrdersController(IOrdersInterface ordersServices)
    {
        _ordersServices = ordersServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _ordersServices.GetAll();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _ordersServices.GetById(id);
        if (order == null)
            return NotFound("Order not found.");
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] Orders order)
    {
        var result = await _ordersServices.AddOrder(order);
        if (result == null)
            return BadRequest("Order could not be added.");
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Orders order)
    {
        try
        {
            if (id <= 0 || id != order.Id)
                return BadRequest("Invalid ID.");

            var result = await _ordersServices.UpdateOrder(id, order);
            if (result == null)
                return NotFound("Order not found.");
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var success = await _ordersServices.DeleteOrder(id);
        if (!success)
            return NotFound("Order not found.");
        return Ok("Order deleted.");
    }
}
