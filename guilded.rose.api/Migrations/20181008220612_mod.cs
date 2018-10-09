using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace guilded.rose.api.Migrations
{
    public partial class mod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Items",
                nullable: false,
                defaultValue: new DateTime(2018, 10, 8, 18, 6, 12, 205, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 10, 8, 17, 59, 29, 979, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Items",
                nullable: false,
                defaultValue: new DateTime(2018, 10, 8, 17, 59, 29, 979, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2018, 10, 8, 18, 6, 12, 205, DateTimeKind.Local));
        }
    }
}
