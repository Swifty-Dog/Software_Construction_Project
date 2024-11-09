using Microsoft.EntityFrameworkCore;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) {}

    public DbSet<Warehouse> Warehouse { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<Locations> Locations {get;set;}
    public DbSet<Item> Items { get; set; }
    public DbSet<Item_group> ItemGroups { get; set; }
    public DbSet<Item_line> ItemLines { get; set; }
    public DbSet<Item_type> ItemTypes { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<Transfers_item> Transfer_Items { get; set; }
    public DbSet<Client> Client { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Inventories_locations> Inventories_Locations { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Shipments_item> Shipments_items { get; set; }
    public DbSet<Orders> Orders { get; set; }


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
            
        // Inventory configuration
        modelBuilder.Entity<Inventory>()
            .HasKey(i => i.Id);  // Primary key for Inventory

        modelBuilder.Entity<Inventory>()
            .HasMany(i => i.Locations)
            .WithOne()
            .HasForeignKey(il => il.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);  // remove locations if inventory is deleted

        // Inventories_locations configuration
        modelBuilder.Entity<Inventories_locations>()
            .HasKey(il => new { il.InventoryId, il.LocationId });  // Composite key using InventoryId and LocationId

        // Shipment configuration
        modelBuilder.Entity<Shipment>()
            .HasKey(s => s.Id);  // Primary key for Shipment

        modelBuilder.Entity<Shipment>()
            .HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(si => si.ShippingId)
            .OnDelete(DeleteBehavior.Cascade);  // remove items if shipment is deleted

        // Transfers_item configuration
        modelBuilder.Entity<Shipments_item>()
            .HasKey(si => new { si.ShippingId, si.ItemId });  // Composite key using ShippingId and ItemId

            


        base.OnModelCreating(modelBuilder);

    }
}
