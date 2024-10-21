using Microsoft.EntityFrameworkCore;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) {}

    public DbSet<Warehouse> Warehouse { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Item_group> ItemGroups { get; set; }
    public DbSet<Item_line> ItemLines { get; set; }
    public DbSet<Item_type> ItemTypes { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<Transfers_item> Transfer_Items { get; set; }
    public DbSet<Client> Client { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Warehouse and Contact configuration
        modelBuilder.Entity<Warehouse>()
            .HasKey(w => w.Id);

        modelBuilder.Entity<Warehouse>()
            .HasOne(w => w.Contact)
            .WithMany()
            .HasForeignKey("ContactId");  // Foreign key for Contact

        modelBuilder.Entity<Contact>()
            .HasKey(c => c.Id);

        // Item entity configuration
        modelBuilder.Entity<Item>()
            .HasKey(i => i.Uid);

        modelBuilder.Entity<Item>()
            .HasOne(i => i.Item_group)
            .WithMany()
            .HasForeignKey("Item_group_id"); // Foreign key to Item_group

        modelBuilder.Entity<Item>()
            .HasOne(i => i.Item_line)
            .WithMany()
            .HasForeignKey("Item_line_id");  // Foreign key to Item_line

        modelBuilder.Entity<Item>()
            .HasOne(i => i.item_type)
            .WithMany()
            .HasForeignKey("Item_type_id");  // Foreign key to Item_type

        modelBuilder.Entity<Item>()
            .HasOne(i => i.supplier_id)
            .WithMany()
            .HasForeignKey("Supplier_id");   // Foreign key to Supplier

        modelBuilder.Entity<Supplier>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Item_group>()
            .HasKey(g => g.Id);

        modelBuilder.Entity<Item_line>()
            .HasKey(l => l.Id);

        modelBuilder.Entity<Item_type>()
            .HasKey(t => t.Id);

        // Transfer configuration
        modelBuilder.Entity<Transfer>()
            .HasKey(t => t.Id);  // Primary key for Transfer

        modelBuilder.Entity<Transfer>()
            .HasMany(t => t.Items)
            .WithOne()
            .HasForeignKey(ti => ti.TransferId)
            .OnDelete(DeleteBehavior.Cascade);  // remove items if transfer is deleted

        // Transfers_item configuration
        modelBuilder.Entity<Transfers_item>()
            .HasKey(ti => new { ti.TransferId, ti.Item_Id });  // Composite key using TransferId and Item_Id

        // Client configuration
        modelBuilder.Entity<Client>()
            .HasKey(c => c.Id);
            
        base.OnModelCreating(modelBuilder);
    }
}
