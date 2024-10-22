using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<MyContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddTransient<Warehouse>();
builder.Services.AddTransient<Contact>();   
builder.Services.AddTransient<Client>();   
builder.Services.AddTransient<Supplier>();
builder.Services.AddTransient<WarehouseServices>(); 
builder.Services.AddTransient<LocationServices>();
builder.Services.AddTransient<TransfersServices>();
builder.Services.AddTransient<Item_groupService>();
builder.Services.AddTransient<ClientServices>();
builder.Services.AddTransient<SuppliersServices>();
builder.Services.AddTransient<ISuppliersInterface, SuppliersServices>();
builder.Services.AddTransient<Inventory>();
builder.Services.AddTransient<Inventories_locations>();
builder.Services.AddTransient<InventoriesServices>();
builder.Services.AddTransient<Shipment>();
builder.Services.AddTransient<Shipments_item>();
builder.Services.AddTransient<ShipmentsServices>();
builder.Services.AddTransient<Item_lineServices>();
builder.Services.AddTransient<Item_TypeServices>();

var app = builder.Build();

app.MapControllers();
app.Urls.Add("http://localhost:5000");
app.MapGet("/", () => "Hello World!");
app.Run();
