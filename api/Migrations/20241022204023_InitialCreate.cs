using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Zip = table.Column<string>(type: "TEXT", nullable: false),
                    Province = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Contact_name = table.Column<string>(type: "TEXT", nullable: false),
                    Contact_phone = table.Column<string>(type: "TEXT", nullable: false),
                    Contact_email = table.Column<string>(type: "TEXT", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Item_id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Item_reference = table.Column<string>(type: "TEXT", nullable: false),
                    Total_on_hand = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_expected = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_ordered = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_allocated = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_available = table.Column<int>(type: "INTEGER", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WarehouseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Source_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Order_date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Request_date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Shipment_date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Shipment_type = table.Column<string>(type: "TEXT", nullable: false),
                    Shipment_status = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    Carrier_code = table.Column<string>(type: "TEXT", nullable: false),
                    Carrier_description = table.Column<string>(type: "TEXT", nullable: false),
                    Service_code = table.Column<string>(type: "TEXT", nullable: false),
                    Payment_type = table.Column<string>(type: "TEXT", nullable: false),
                    Transfer_mode = table.Column<string>(type: "TEXT", nullable: false),
                    Total_package_count = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_package_weight = table.Column<float>(type: "REAL", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Address_extra = table.Column<string>(type: "TEXT", nullable: false),
                    Zip_code = table.Column<string>(type: "TEXT", nullable: false),
                    Province = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Contact_name = table.Column<string>(type: "TEXT", nullable: false),
                    Phonenumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Transfer_from = table.Column<int>(type: "INTEGER", nullable: true),
                    Transfer_to = table.Column<int>(type: "INTEGER", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Zip = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    ContactId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouse_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories_Locations",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories_Locations", x => new { x.InventoryId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_Inventories_Locations_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments_items",
                columns: table => new
                {
                    ShippingId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments_items", x => new { x.ShippingId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_Shipments_items_Shipments_ShippingId",
                        column: x => x.ShippingId,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Uid = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Short_Description = table.Column<string>(type: "TEXT", nullable: false),
                    Upc_code = table.Column<string>(type: "TEXT", nullable: false),
                    Model_number = table.Column<string>(type: "TEXT", nullable: false),
                    Commodity_code = table.Column<string>(type: "TEXT", nullable: false),
                    Item_line_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Item_group_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Item_type_id = table.Column<int>(type: "INTEGER", nullable: false),
                    unit_purchase_quantity = table.Column<string>(type: "TEXT", nullable: false),
                    unit_order_quantity = table.Column<string>(type: "TEXT", nullable: false),
                    pack_order_quantity = table.Column<string>(type: "TEXT", nullable: false),
                    Supplier_id = table.Column<int>(type: "INTEGER", nullable: false),
                    supplier_codeId = table.Column<int>(type: "INTEGER", nullable: false),
                    supplier_part_numberId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Items_ItemGroups_Item_group_id",
                        column: x => x.Item_group_id,
                        principalTable: "ItemGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_ItemLines_Item_line_id",
                        column: x => x.Item_line_id,
                        principalTable: "ItemLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_Item_type_id",
                        column: x => x.Item_type_id,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Suppliers_Supplier_id",
                        column: x => x.Supplier_id,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Suppliers_supplier_codeId",
                        column: x => x.supplier_codeId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Suppliers_supplier_part_numberId",
                        column: x => x.supplier_part_numberId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transfer_Items",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "INTEGER", nullable: false),
                    Item_Id = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfer_Items", x => new { x.TransferId, x.Item_Id });
                    table.ForeignKey(
                        name: "FK_Transfer_Items_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_Item_group_id",
                table: "Items",
                column: "Item_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Item_line_id",
                table: "Items",
                column: "Item_line_id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Item_type_id",
                table: "Items",
                column: "Item_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_supplier_codeId",
                table: "Items",
                column: "supplier_codeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Supplier_id",
                table: "Items",
                column: "Supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_supplier_part_numberId",
                table: "Items",
                column: "supplier_part_numberId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_ContactId",
                table: "Warehouse",
                column: "ContactId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Inventories_Locations");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Shipments_items");

            migrationBuilder.DropTable(
                name: "Transfer_Items");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "ItemGroups");

            migrationBuilder.DropTable(
                name: "ItemLines");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Contact");
        }
    }
}
