using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace rsH60Store.Models;

public partial class H60assignmentDbRsContext : DbContext
{
    
    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    
    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }

    public H60assignmentDbRsContext(DbContextOptions<H60assignmentDbRsContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=cssql.cegep-heritage.qc.ca;Database=test_H60AssignmentDB_rs;User id=RSOMERS;Password=password;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("rsH60Store.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ProductID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<decimal?>("BuyPrice")
                        .HasColumnType("numeric(8, 2)");

                    b.Property<string>("Description")
                        .HasMaxLength(80)
                        .IsUnicode(false)
                        .HasColumnType("varchar(80)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manufacturer")
                        .HasMaxLength(80)
                        .IsUnicode(false)
                        .HasColumnType("varchar(80)");

                    b.Property<int>("ProdCatId")
                        .HasColumnType("int");

                    b.Property<decimal?>("SellPrice")
                        .HasColumnType("numeric(8, 2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex(new[] { "ProdCatId" }, "IX_Product_ProdCatId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("rsH60Store.Models.ProductCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CategoryID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("ProdCat")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)");

                    b.HasKey("CategoryId");

                    b.ToTable("ProductCategory", (string)null);
                });

            modelBuilder.Entity("rsH60Store.Models.Product", b =>
                {
                    b.HasOne("rsH60Store.Models.ProductCategory", "ProdCat")
                        .WithMany("Products")
                        .HasForeignKey("ProdCatId")
                        .IsRequired()
                        .HasConstraintName("FK_Product_ProductCategory");

                    b.Navigation("ProdCat");
                });
            
                // Setup relationship between Customer and AspNetUsers
                modelBuilder.Entity<Customer>()
                    .HasOne<IdentityUser>(c => c.User)
                    .WithMany()
                    .HasForeignKey(c => c.UserId);

            modelBuilder.Entity("rsH60Store.Models.ProductCategory", b =>
                {
                    b.Navigation("Products");
                });
            
                modelBuilder.Entity<ShoppingCart>()
                    .HasKey(sc => sc.CartId);

                modelBuilder.Entity<CartItem>()
                    .HasOne(ci => ci.Product)
                    .WithMany() // Assuming Product does not have a collection of CartItems
                    .HasForeignKey(ci => ci.ProductId);

                modelBuilder.Entity<ShoppingCart>()
                    .HasOne(sc => sc.Customer)
                    .WithMany(c => c.ShoppingCarts)
                    .HasForeignKey(sc => sc.CustomerId);

                modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId);

                modelBuilder.Entity<OrderItem>()
                    .HasOne(oi => oi.Product)
                    .WithMany() // Assuming Product does not have a collection of OrderItems
                    .HasForeignKey(oi => oi.ProductId);

                modelBuilder.Entity<Order>()
                    .HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId);
                
                // Seed Customers
    modelBuilder.Entity<Customer>().HasData(
        new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890", Province = "ON", CreditCard = "1234567812345678" },
        new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "0987654321", Province = "QC", CreditCard = "2345678923456789" },
        new Customer { CustomerId = 3, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", PhoneNumber = "1122334455", Province = "BC", CreditCard = "3456789034567890" }
    );

    // Seed Product Categories
    modelBuilder.Entity<ProductCategory>().HasData(
        new ProductCategory { CategoryId = 1, ProdCat = "Sneakers" },
        new ProductCategory { CategoryId = 2, ProdCat = "Hoodies" },
        new ProductCategory { CategoryId = 3, ProdCat = "T-Shirts" },
        new ProductCategory { CategoryId = 4, ProdCat = "Bottoms" },
        new ProductCategory { CategoryId = 5, ProdCat = "Outerwear" },
        new ProductCategory { CategoryId = 6, ProdCat = "Bags" }
    );

    // Seed Products
    modelBuilder.Entity<Product>().HasData(
        new Product { ProductId = 1, ProdCatId = 1, Description = "Air Jordan 1 Low (Reverse Mocha)", Manufacturer = "Nike", Stock = 100, BuyPrice = 1200.00m, SellPrice = 1250.00m, ImageUrl = "/images/reversem.png" },
        new Product { ProductId = 2, ProdCatId = 3, Description = "Fear of God Essentials Tee 'Light Oatmeal'", Manufacturer = "Essentials", Stock = 50, BuyPrice = 150.00m, SellPrice = 200.00m, ImageUrl = "/images/tshirt.png" },
        new Product { ProductId = 3, ProdCatId = 2, Description = "Sp5der Web Hoodie 'Sky Blue'", Manufacturer = "Sp5der", Stock = 200, BuyPrice = 300.00m, SellPrice = 350.00m, ImageUrl = "/images/hoodie.png" }
    );

    // Seed Shopping Cart and Cart Items
    modelBuilder.Entity<ShoppingCart>().HasData(
        new ShoppingCart { CartId = 1, CustomerId = 1, DateCreated = DateTime.Now }
    );

    modelBuilder.Entity<CartItem>().HasData(
        new CartItem { CartItemId = 1, CartId = 1, ProductId = 1, Quantity = 2, Price = 2400.00m },
        new CartItem { CartItemId = 2, CartId = 1, ProductId = 2, Quantity = 1, Price = 200.0m }
    );

    // Seed Orders and Order Items
    modelBuilder.Entity<Order>().HasData(
        new Order { OrderId = 1, CustomerId = 2, DateCreated = DateTime.Now, DateFulfilled = DateTime.Now, Total = 499.97m, Taxes = 49.99m }
    );

    modelBuilder.Entity<OrderItem>().HasData(
        new OrderItem { OrderItemId = 1, OrderId = 1, ProductId = 1, Quantity = 3, Price = 3600.00m },
        new OrderItem { OrderItemId = 2, OrderId = 1, ProductId = 2, Quantity = 1, Price = 150.00m }
    );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
