
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) {}
    public DbSet<Warehouse> Warehouse { get; set; }
    public DbSet<Contact> Contact { get; set; }
    // here comes the table names based on the class names (such as shipments, clients, items ect)
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Warehouse>()
            .HasOne(w => w.Contact)
            .WithMany()
            .HasForeignKey("ContactId");  // Optional: if you are setting up a foreign key
    }

}