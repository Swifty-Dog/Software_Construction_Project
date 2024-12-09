using Microsoft.EntityFrameworkCore;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) {}

    public DbSet<Warehouse> Warehouse { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<Locations> Locations {get;set;}
    public DbSet<Item> Items { get; set; }
    public DbSet<Item_group> ItemGroups { get; set; }
    public DbSet<ItemLine> ItemLines { get; set; }
    public DbSet<Item_type> ItemTypes { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<TransfersItem> TransferItems { get; set; }
    public DbSet<Client> Client { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<InventoriesLocations> InventoriesLocations { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentsItem> ShipmentsItems { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<User> Users { get; set; }


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
    
        modelBuilder.Entity<Warehouse>()
            .HasMany(w => w.Locations)
            .WithOne()
            .HasForeignKey(l => l.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);   

        // Item entity configuration
        modelBuilder.Entity<Item>()
            .HasKey(i => i.Uid);

        modelBuilder.Entity<Supplier>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Item_group>()
            .HasKey(g => g.Id);

        modelBuilder.Entity<ItemLine>()
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
        modelBuilder.Entity<TransfersItem>()
            .HasKey(ti => new { ti.TransferId, ti.ItemId });  // Composite key using TransferId and Item_Id

        modelBuilder.Entity<Orders_Item>()
            .HasKey(o => new { o.OrderId});

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

        // InventoriesLocations configuration
        modelBuilder.Entity<InventoriesLocations>()
            .HasKey(il => new { il.InventoryId, il.LocationId });  // Composite key using inventoryId and locationId

        // Shipment configuration
        modelBuilder.Entity<Shipment>()
            .HasKey(s => s.Id);  // Primary key for Shipment

        modelBuilder.Entity<Shipment>()
            .HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(si => si.ShippingId)
            .OnDelete(DeleteBehavior.Cascade);  // remove items if shipment is deleted

        // Transfers_item configuration
        modelBuilder.Entity<ShipmentsItem>()
            .HasKey(si => new { si.ShippingId, si.ItemId });  // Composite key using ShippingId and ItemId

        modelBuilder.Entity<User>()
            .HasMany(u => u.EndpointAccesses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1, // Required to be unique
                ApiKey = "a1b2c3d4e5",
                AppName = "CargoHUB Dashboard Full",
                HasFullAccess = true
            }
        );
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 2, 
                ApiKey = "f6g7h8i9j0",
                AppName = "CargoHUB Dashboard 2",
                HasFullAccess = false 
            }
        );

        // Seed endpoint access data
        modelBuilder.Entity<EndpointAccess>().HasData(
            new EndpointAccess { Id = 1, Endpoint = "warehouses", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 2, Endpoint = "locations", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 3, Endpoint = "transfers",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 4, Endpoint = "items",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 5, Endpoint = "item_lines", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 6, Endpoint = "item_groups", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 7, Endpoint = "item_types", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 8, Endpoint = "suppliers",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 9, Endpoint = "orders", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 10, Endpoint = "clients", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 11, Endpoint = "shipments",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 },
            new EndpointAccess { Id = 12, Endpoint = "inventories",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 2 }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 4, 
                ApiKey = "InventoryManager",
                AppName = "Inventory Manager",
                HasFullAccess = false 
            }
        );   
        modelBuilder.Entity<EndpointAccess>().HasData(
            new EndpointAccess { Id = 25, Endpoint = "warehouses", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 4 },
            new EndpointAccess { Id = 26, Endpoint = "locations", CanGet = true, CanPost = true, CanPut = true, CanDelete = true, UserId = 4 },
            new EndpointAccess { Id = 27, Endpoint = "transfers",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 4 },
            new EndpointAccess { Id = 28, Endpoint = "items",  CanGet = true, CanPost = true, CanPut = false, CanDelete = false, UserId = 4 },
            new EndpointAccess { Id = 29, Endpoint = "item_lines", CanGet = true, CanPost = true, CanPut = true, CanDelete = true, UserId = 4 },
            new EndpointAccess { Id = 30, Endpoint = "item_groups", CanGet = true, CanPost = true, CanPut = true, CanDelete = true, UserId = 4 },
            new EndpointAccess { Id = 31, Endpoint = "item_types", CanGet = true, CanPost = true, CanPut = true, CanDelete = true, UserId = 4 },
            new EndpointAccess { Id = 32, Endpoint = "suppliers",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 4 },
            new EndpointAccess { Id = 33, Endpoint = "orders", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 4 },
            new EndpointAccess { Id = 34, Endpoint = "clients", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 4 },
            new EndpointAccess { Id = 35, Endpoint = "shipments",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 4 },
            new EndpointAccess { Id = 36, Endpoint = "inventories",  CanGet = true, CanPost = true, CanPut = true, CanDelete = true, UserId = 4 }
        );
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 5, 
                ApiKey = "FloorManager",
                AppName = "Floor Manager",
                HasFullAccess = false 
            }
        );

        modelBuilder.Entity<EndpointAccess>().HasData(
            new EndpointAccess { Id = 37, Endpoint = "warehouses", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 38, Endpoint = "locations", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 39, Endpoint = "transfers",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 40, Endpoint = "items",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 41, Endpoint = "item_lines", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 42, Endpoint = "item_groups", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 43, Endpoint = "item_types", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 44, Endpoint = "suppliers",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 45, Endpoint = "orders", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 46, Endpoint = "clients", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 47, Endpoint = "shipments",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 },
            new EndpointAccess { Id = 48, Endpoint = "inventories",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 5 }
        );

        base.OnModelCreating(modelBuilder);
    }
}
