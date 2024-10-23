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


}
