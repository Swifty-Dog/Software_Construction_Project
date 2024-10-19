using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(("/api/v1/locations"))]

public class LocationController : ControllerBase
{
    private readonly MyContext _mycontext;

    public LocationController(MyContext mycontext)
    {
        _mycontext = mycontext;
    }


}