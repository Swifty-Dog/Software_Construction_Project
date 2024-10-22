using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<MyContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddTransient<Warehouse>();
builder.Services.AddTransient<Contact>();   
builder.Services.AddTransient<WarehouseServices>(); 
builder.Services.AddTransient<TransfersServices>();
builder.Services.AddTransient<Inventory>();
builder.Services.AddTransient<Inventories_locations>();
builder.Services.AddTransient<InventoriesServices>();

var app = builder.Build();

app.MapControllers();

app.Urls.Add("http://localhost:5000");
app.MapGet("/", () => "Hello World!");
app.Run();
