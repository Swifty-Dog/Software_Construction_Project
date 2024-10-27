using Microsoft.EntityFrameworkCore;

public class Authentication
{
    private readonly RequestDelegate _next;

    public Authentication(RequestDelegate next){
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context){
        if (!context.Request.Headers.TryGetValue("Api-Key", out var apiKeyValues)){
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        // Convert apiKeyValues to a string
        string apiKey = apiKeyValues.ToString();

        var dbContext = context.RequestServices.GetRequiredService<MyContext>();
        var user = dbContext.Users
                            .Include(u => u.EndpointAccesses)
                            .SingleOrDefault(u => u.ApiKey == apiKey);

        if (user == null){
            context.Response.StatusCode = 403; 
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        // Check if the user has full access
        if (user.HasFullAccess){
            await _next(context);
            return;
        }

        // If not full access, check specific endpoint and method permissions
        var path = context.Request.Path.Value.Split('/')[2]; // Assuming path format is /api/{endpoint}/{action}
        var method = context.Request.Method.ToUpper();

        var endpointAccess = user.EndpointAccesses.FirstOrDefault(e => e.Endpoint == path);

        bool hasPermission = method switch{
            "GET" => endpointAccess?.CanGet ?? false,
            "POST" => endpointAccess?.CanPost ?? false,
            "PUT" => endpointAccess?.CanPut ?? false,
            "DELETE" => endpointAccess?.CanDelete ?? false,
            _ => false
        };

        if (!hasPermission){
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Permission denied.");
            return;
        }

        await _next(context);
    }
}
