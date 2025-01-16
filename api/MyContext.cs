using Microsoft.EntityFrameworkCore;

public class MyContext : DbContext
{
    // public MyContext() { }

    public MyContext(DbContextOptions<MyContext> options) : base(options) {}

    public /* virtual */ DbSet<Warehouse> Warehouse { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<Locations> Locations {get;set;}
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemGroup> ItemGroups { get; set; }
    public DbSet<ItemLine> ItemLines { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<TransfersItem> TransferItems { get; set; }
    public virtual DbSet<Client> Client { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    // public DbSet<InventoriesLocations> InventoriesLocations { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ShipmentsItem> ShipmentsItems { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<OrdersItem> OrdersItems { get; set; }
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

        modelBuilder.Entity<ItemGroup>()
            .HasKey(g => g.Id);

        modelBuilder.Entity<ItemLine>()
            .HasKey(l => l.Id);

        modelBuilder.Entity<ItemType>()
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

        modelBuilder.Entity<OrdersItem>()
            .HasKey(o => new { o.OrderId });  // Composite key using OrderId and ItemId

        // Client configuration
        modelBuilder.Entity<Client>()
            .HasKey(c => c.Id);
            
        // Inventory configuration
        modelBuilder.Entity<Inventory>()
            .HasKey(i => i.Id);  // Primary key for Inventory
        
        /*
        modelBuilder.Entity<Inventory>()
            .HasMany(i => i.Locations)
            .WithOne()
            .HasForeignKey(il => il.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);  // remove locations if inventory is deleted
             
        // InventoriesLocations configuration
        modelBuilder.Entity<InventoriesLocations>()
            .HasKey(il => new { il.InventoryId, il.LocationId });  // Composite key using inventoryId and locationId
        */

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
                Id = 3, // Unique Id for this user
                ApiKey = "WarehouseManager",
                AppName = "Warehouse Manager",
                HasFullAccess = false // Indicates limited access
            }
        );

        // Seed endpoint access data
        modelBuilder.Entity<EndpointAccess>().HasData(
            new EndpointAccess { Id = 13, Endpoint = "warehouses", CanGet = true, CanPost = true, CanPut = false, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 14, Endpoint = "locations", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 15, Endpoint = "transfers",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 16, Endpoint = "items",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 17, Endpoint = "item_lines", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 18, Endpoint = "item_groups", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 19, Endpoint = "item_types", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 20, Endpoint = "suppliers",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 21, Endpoint = "orders", CanGet = true, CanPost = true, CanPut = true, CanDelete = true, UserId = 3 },
            new EndpointAccess { Id = 22, Endpoint = "clients", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 23, Endpoint = "shipments",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 },
            new EndpointAccess { Id = 24, Endpoint = "inventories",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 3 }
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

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 6, 
                ApiKey = "Operative",
                AppName = "Operative",
                HasFullAccess = false 
            }
        );
        modelBuilder.Entity<EndpointAccess>().HasData(
            new EndpointAccess { Id = 49, Endpoint = "warehouses", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 50, Endpoint = "locations", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 51, Endpoint = "transfers",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 52, Endpoint = "items",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 53, Endpoint = "item_lines", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 54, Endpoint = "item_groups", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 55, Endpoint = "item_types", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 56, Endpoint = "suppliers",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 57, Endpoint = "orders", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 58, Endpoint = "clients", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 59, Endpoint = "shipments",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 },
            new EndpointAccess { Id = 60, Endpoint = "inventories",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 6 }   
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 7, 
                ApiKey = "Supervisor",
                AppName = "Supervisor",
                HasFullAccess = false 
            }
        );
        modelBuilder.Entity<EndpointAccess>().HasData(
            new EndpointAccess { Id = 61, Endpoint = "warehouses", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 62, Endpoint = "locations", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 63, Endpoint = "transfers",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 64, Endpoint = "items",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 65, Endpoint = "item_lines", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 66, Endpoint = "item_groups", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 67, Endpoint = "item_types", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 68, Endpoint = "suppliers",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 69, Endpoint = "orders", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 70, Endpoint = "clients", CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 71, Endpoint = "shipments",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 },
            new EndpointAccess { Id = 72, Endpoint = "inventories",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 7 }   
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 8, 
                ApiKey = "Analyst",
                AppName = "Analyst",
                HasFullAccess = false 
            }
        );

        modelBuilder.Entity<EndpointAccess>().HasData(
            new EndpointAccess { Id = 73, Endpoint = "warehouses", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 74, Endpoint = "locations", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 75, Endpoint = "transfers",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 76, Endpoint = "items",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 77, Endpoint = "item_lines", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 78, Endpoint = "item_groups", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 79, Endpoint = "item_types", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 80, Endpoint = "suppliers",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 81, Endpoint = "orders", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 82, Endpoint = "clients", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 83, Endpoint = "shipments",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 },
            new EndpointAccess { Id = 84, Endpoint = "inventories",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 8 }
        );

         modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 9, 
                ApiKey = "Logistics",
                AppName = "Logistics",
                HasFullAccess = false 
            }
        );    

        modelBuilder.Entity<EndpointAccess>().HasData(
            new EndpointAccess { Id = 85, Endpoint = "warehouses", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 86, Endpoint = "locations", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 87, Endpoint = "transfers",  CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 88, Endpoint = "items",  CanGet = true, CanPost = true, CanPut = false, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 89, Endpoint = "item_lines", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 90, Endpoint = "item_groups", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 91, Endpoint = "item_types", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 92, Endpoint = "suppliers",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 93, Endpoint = "orders", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 94, Endpoint = "clients", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 95, Endpoint = "shipments",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 9 },
            new EndpointAccess { Id = 96, Endpoint = "inventories",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 9 }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 10,
                ApiKey = "Sales",
                AppName = "Sales",
                HasFullAccess = false
            }
        );

        modelBuilder.Entity<EndpointAccess>().HasData(
            new EndpointAccess { Id = 97, Endpoint = "warehouses", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 98, Endpoint = "locations", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 99, Endpoint = "transfers",  CanGet = false, CanPost = false, CanPut = false, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 100, Endpoint = "items",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 101, Endpoint = "item_lines", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 102, Endpoint = "item_groups", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 103, Endpoint = "item_types", CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 104, Endpoint = "suppliers",  CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 105, Endpoint = "orders", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 106, Endpoint = "clients", CanGet = true, CanPost = true, CanPut = true, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 107, Endpoint = "shipments",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 10 },
            new EndpointAccess { Id = 108, Endpoint = "inventories",  CanGet = true, CanPost = false, CanPut = false, CanDelete = false, UserId = 10 }
        );
        
        base.OnModelCreating(modelBuilder);
    }
}
