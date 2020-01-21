using System;
using System.IO;
using GildedRose.Entities.Inventory;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GildedRose.Data.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Category = table.Column<string>(nullable: false),
                    SellIn = table.Column<int>(nullable: false),
                    Quality = table.Column<int>(nullable: false),
                    InventoryAddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryHistory",
                columns: table => new
                {
                    HistoryId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    SellIn = table.Column<int>(nullable: false),
                    Quality = table.Column<int>(nullable: false),
                    InventoryAddedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Action = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryHistory", x => x.HistoryId);
                });

            SeedInventoryData(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "InventoryAging");

            migrationBuilder.DropTable(
                name: "InventoryHistory");
        }

        private void SeedInventoryData(MigrationBuilder migrationBuilder)
        {
            /**********************************************************************************
             *
             * This is a crude import of the data and lacks full-blown error handling
             * but I am using the assumption that the initial data file would simply be a
             * one-time import and would not be used again in the future.
             *
             * I would normally handle a one-time data migration in a different way
             * and typically outside of the core code base (e.g. via deployment script).
             *
             * For this exercise, though, I use an Entity Framework migration to both
             * initialize the database structure and seed the Inventory table from the
             * data in the inventory.txt file by using built-in migration capabilities.
             * For this exercise, it seemed to be an apropos tool.
             *
             **********************************************************************************/
            var seedDataFolder = DataContextHelper.GetRootPath();
            var seedDataFile = $"{seedDataFolder}\\inventory.txt";

            if (!File.Exists(seedDataFile))
            {
                throw new FileNotFoundException("Initial inventory file was not found at: {initialInventoryPath}");
            }

            using (var fs = new FileStream(seedDataFile, FileMode.Open, FileAccess.Read))
            using (var sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var columns = line.Split(',');
                        var id = Guid.NewGuid();
                        var today = DateTime.Today;

                        // Add the inventory record
                        migrationBuilder.InsertData(
                            table: "Inventory",
                            columns: new[] { "Id", "Name", "Category", "SellIn", "Quality", "InventoryAddedDate" },
                            values: new object[] { id, columns[0], columns[1], int.Parse(columns[2]), int.Parse(columns[3]), today });

                        // Add the history/audit record
                        migrationBuilder.InsertData(
                            table: "InventoryHistory",
                            columns: new[] { "Id", "Name", "Category", "SellIn", "Quality", "InventoryAddedDate", "LastModifiedDate", "Action" },
                            values: new object[] { id, columns[0], columns[1], int.Parse(columns[2]), int.Parse(columns[3]), today, DateTime.Now, Convert.ToInt32(InventoryHistoryAction.Created) });
                    }
                }
            }
        }
    }
}
