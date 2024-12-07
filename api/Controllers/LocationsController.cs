using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(("/api/v1/locations"))]

public class LocationController : ControllerBase
{
    private readonly LocationServices _locationServices;

    public LocationController(LocationServices locationServices)
    {
        _locationServices = locationServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLocations()
    {
        var locations = await _locationServices.GetLocations();
        return Ok(locations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLocation(int id)
    {
        var location = await _locationServices.GetLocationById(id);
        if (location == null)
        {
            return NotFound("Location not found.");
        }
        return Ok(location);
    }

    [HttpPost]
    public async Task<IActionResult> AddLocation([FromBody] Locations location)
    {
        try
        {
            var result = await _locationServices.AddLocation(location);
            if (result == null)
            {
                return BadRequest("Location could not be added or already exists.");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(int id, [FromBody] Locations location)
    {
        try
        {   
            if(id <= 0 || location.Id != id)
            {
                return BadRequest("Invalid location ID or location ID does not match the location object.");
            }
            var result = await _locationServices.UpdateLocation(id, location);
            if (result == null)
            {
                return NotFound("Location not found or could not be updated.");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        try
        {
            var deleteLocation = await _locationServices.DeleteLocation(id);
            if (!deleteLocation)
            {
                return NotFound("Location not found or could not be deleted.");
            }
            //return NoContent();
            return Ok("Deleted the location");
        }   
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
