using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Internet_veikals.Migrations
{
    public partial class aaaab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "products");
        }
    }
}
