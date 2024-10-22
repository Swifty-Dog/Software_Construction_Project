using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build(); //waarom is dit weg in main?

builder.Services.AddControllers();
builder.Services.AddDbContext<MyContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<LocationServices>();

// Register services
builder.Services.AddTransient<Warehouse>();
builder.Services.AddTransient<Contact>();   
builder.Services.AddTransient<Supplier>();
builder.Services.AddTransient<WarehouseServices>(); 
builder.Services.AddTransient<TransfersServices>();
builder.Services.AddTransient<SuppliersServices>();

app.MapControllers();
app.Urls.Add("http://localhost:5000");
app.MapGet("/", () => "Hello World!");
app.Run();
