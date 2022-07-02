using Microsoft.EntityFrameworkCore.Migrations;

namespace Internet_veikals.Migrations
{
    public partial class a2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShippingType",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShippingType",
                table: "Orders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
