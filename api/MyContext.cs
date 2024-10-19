
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) {}
    public DbSet<Warehouses> Warehouses { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<Locations> Locations {get;set;}
    // here comes the table names based on the class names (such as shipments, clients, items ect)

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
        //modelBuilder.Entity<Locations>()
            //.HasOne(l => l.Warehouse) // Specifies that Locations has one Warehouse
            //.WithMany() // Specifies that Warehouse can have many Locations
            //.HasForeignKey(l => l.WarehouseId); // Specifies the foreign key property
    //}
}