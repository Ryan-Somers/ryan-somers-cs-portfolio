using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsH60Customer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ShoppingCarts",
                type: "nvarchar(max)", // Adjust type if needed
                nullable: false,
                defaultValue: ""); // Ensure non-nullability
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCarts");
        }
    }
}
