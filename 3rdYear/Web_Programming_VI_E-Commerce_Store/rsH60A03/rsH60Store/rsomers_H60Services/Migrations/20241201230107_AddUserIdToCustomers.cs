using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsomers_H60Services.Migrations
{
    public partial class AddUserIdToCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the UserId column to the Customers table
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // If you want to ensure UserId is unique, you can add an index
            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the UserId column from the Customers table
            migrationBuilder.DropIndex(
                name: "IX_Customers_UserId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Customers");
        }
    }
}