
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) {}

    // here comes the table names based on the class names (such as shipments, clients, items ect)
    
}