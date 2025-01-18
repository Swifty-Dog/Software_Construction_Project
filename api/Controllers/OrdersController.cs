using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("/api/v1/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersInterface _ordersServices;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrdersInterface ordersServices, ILogger<OrdersController>? logger = null)
    {
        _ordersServices = ordersServices;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _ordersServices.GetAll();
        if (orders == null)
        {
            _logger?.LogInformation("GET /api/v1/orders: No orders found.");
            return NotFound("No orders found.");
        }

        _logger?.LogInformation("GET /api/v1/orders: Retrieved all orders.");
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _ordersServices.GetById(id);
        if (order == null)
        {
            _logger?.LogInformation("GET /api/v1/orders/{id}: Order with ID {id} not found.", id);
            return NotFound("Order not found.");
        }

        _logger?.LogInformation("GET /api/v1/orders/{Id}: Order retrieved successfully. Details: {@Order}", id, order);
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] Orders order)
    {
        try
        {
            var result = await _ordersServices.AddOrder(order);
            if (result == null)
            {
                _logger?.LogInformation("POST /api/v1/orders: Order could not be added. Details: {@Order}", order);
                return BadRequest("Order could not be added.");
            }

            _logger?.LogInformation("POST /api/v1/orders: Order added successfully. Details: {@Order}", result);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "POST /api/v1/orders: An error occurred while adding the order.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Orders order)
    {
        try
        {
            var oldOrder = await _ordersServices.GetById(id);
            var oldOrderSnapshot = JsonConvert.DeserializeObject<Orders>(JsonConvert.SerializeObject(oldOrder));

            if (id <= 0 || id != order.Id)
            {
                _logger?.LogInformation("PUT /api/v1/orders/{Id}: Invalid or mismatched ID in request body.", id);
                return BadRequest("Invalid ID.");
            }

            var result = await _ordersServices.UpdateOrder(id, order);
            if (result == null)
            {
                _logger?.LogInformation("PUT /api/v1/orders/{Id}: Order with ID {Id} not found or could not be updated.", id);
                return NotFound("Order not found.");
            }

            _logger?.LogInformation(
                "PUT /api/v1/orders/{Id}: Order updated successfully. Old Order: {@OldOrder}, New Order: {@NewOrder}", 
                id, oldOrderSnapshot, result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "PUT /api/v1/orders/{Id}: An error occurred while updating the order.", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            var orderToDelete = await _ordersServices.GetById(id);
            var orderSnapshot = JsonConvert.DeserializeObject<Orders>(JsonConvert.SerializeObject(orderToDelete));

            if (orderToDelete == null)
            {
                _logger?.LogInformation("DELETE /api/v1/orders/{Id}: Order with ID {Id} not found.", id);
                return NotFound("Order not found.");
            }

            var success = await _ordersServices.DeleteOrder(id);
            if (!success)
            {
                _logger?.LogInformation("DELETE /api/v1/orders/{Id}: Order with ID {Id} could not be deleted.", id);
                return BadRequest("Order could not be deleted.");
            }

            _logger?.LogInformation("DELETE /api/v1/orders/{Id}: Order deleted successfully. Deleted Order: {@Order}", id, orderSnapshot);
            return Ok("Order deleted.");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "DELETE /api/v1/orders/{Id}: An error occurred while deleting the order.", id);
            return BadRequest(ex.Message);
        }
    }
}
