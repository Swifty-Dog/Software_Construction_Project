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
    public async Task<IActionResult> GetAllOrders()
    {
        // Implementation goes here
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        // Implementation goes here
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] Order order)
    {
        // Implementation goes here
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
    {
        // Implementation goes here
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        // Implementation goes here
    }
}
