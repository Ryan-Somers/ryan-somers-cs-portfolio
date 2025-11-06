using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace rsH60Store.Migrations
{
    /// <inheritdoc />
    public partial class addseededdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreditCard", "Email", "FirstName", "LastName", "PhoneNumber", "Province" },
                values: new object[,]
                {
                    { 1, "1234567812345678", "john.doe@example.com", "John", "Doe", "1234567890", "ON" },
                    { 2, "2345678923456789", "jane.smith@example.com", "Jane", "Smith", "0987654321", "QC" },
                    { 3, "3456789034567890", "alice.johnson@example.com", "Alice", "Johnson", "1122334455", "BC" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "CategoryID", "ProdCat" },
                values: new object[,]
                {
                    { 1, "Sneakers" },
                    { 2, "Hoodies" },
                    { 3, "T-Shirts" },
                    { 4, "Bottoms" },
                    { 5, "Outerwear" },
                    { 6, "Bags" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "DateCreated", "DateFulfilled", "Taxes", "Total" },
                values: new object[] { 1, 2, new DateTime(2024, 9, 13, 1, 4, 47, 35, DateTimeKind.Local).AddTicks(7500), new DateTime(2024, 9, 13, 1, 4, 47, 35, DateTimeKind.Local).AddTicks(7500), 49.99m, 499.97m });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductID", "BuyPrice", "Description", "ImageUrl", "Manufacturer", "ProdCatId", "SellPrice", "Stock" },
                values: new object[,]
                {
                    { 1, 1200.00m, "Air Jordan 1 Low (Reverse Mocha)", "/images/reversem.png", "Nike", 1, 1250.00m, 100 },
                    { 2, 150.00m, "Fear of God Essentials Tee 'Light Oatmeal'", "/images/tshirt.png", "Essentials", 3, 200.00m, 50 },
                    { 3, 300.00m, "Sp5der Web Hoodie 'Sky Blue'", "/images/hoodie.png", "Sp5der", 2, 350.00m, 200 }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "CartId", "CustomerId", "DateCreated" },
                values: new object[] { 1, 1, new DateTime(2024, 9, 13, 1, 4, 47, 35, DateTimeKind.Local).AddTicks(7440) });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "CartItemId", "CartId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 2400.00m, 1, 2 },
                    { 2, 1, 200.0m, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 3600.00m, 1, 3 },
                    { 2, 1, 150.00m, 2, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ShoppingCarts",
                keyColumn: "CartId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 3);
        }
    }
}
