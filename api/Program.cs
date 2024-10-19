using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<Warehouses>();
builder.Services.AddTransient<Contact>();
builder.Services.AddTransient<LocationServices>();

var app = builder.Build();
app.Urls.Add("http://localhost:5000");
app.MapGet("/" , () => "Hello");
app.MapControllers();
app.Run();


