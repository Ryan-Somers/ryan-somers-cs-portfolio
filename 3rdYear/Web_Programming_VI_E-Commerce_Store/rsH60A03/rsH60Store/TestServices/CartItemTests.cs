using Microsoft.EntityFrameworkCore;
using Moq;
using rsomers_H60Services.Models;
using rsomers_H60Services.Models.Interfaces;
using rsomers_H60Services.Models.Repositories;

namespace TestShoppingCart;

public class CartItemTests
{
    private ServicesDBContext GetDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ServicesDBContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            return new ServicesDBContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddCartItem()
        {
            // Arrange
            var dbContext = GetDbContext(Guid.NewGuid().ToString());
            var repository = new CartItemRepository(dbContext);

            var product = new Product
            {
                ProductId = 1,
                ProdCatId = 2,
                Description = "Test Product",
                Manufacturer = "Test Manufacturer",
                Stock = 10,
                BuyPrice = 5.0m,
                SellPrice = 10.0m
            };

            var cart = new ShoppingCart
            {
                CustomerId = 1,
                DateCreated = DateTime.UtcNow
            };

            dbContext.Products.Add(product);
            dbContext.ShoppingCarts.Add(cart);
            await dbContext.SaveChangesAsync();

            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = product.ProductId,
                Quantity = 1,
                Price = 10.0m
            };

            // Act
            await repository.AddAsync(cartItem);

            // Assert
            var addedCartItem = await dbContext.CartItems.FirstOrDefaultAsync();
            Assert.NotNull(addedCartItem);
            Assert.Equal(cartItem.Quantity, addedCartItem.Quantity);
            Assert.Equal(cartItem.Price, addedCartItem.Price);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCartItem()
        {
            // Arrange
            var dbContext = GetDbContext(Guid.NewGuid().ToString());
            var repository = new CartItemRepository(dbContext);

            var product = new Product
            {
                ProductId = 1,
                ProdCatId = 2,
                Description = "Test Product",
                Manufacturer = "Test Manufacturer",
                Stock = 10,
                BuyPrice = 5.0m,
                SellPrice = 10.0m
            };

            var cart = new ShoppingCart
            {
                CustomerId = 1,
                DateCreated = DateTime.UtcNow
            };

            dbContext.Products.Add(product);
            dbContext.ShoppingCarts.Add(cart);
            await dbContext.SaveChangesAsync(); // Save ShoppingCart to generate CartId

            var cartItem = new CartItem
            {
                CartId = cart.CartId, // Use the generated CartId
                ProductId = product.ProductId,
                Quantity = 1,
                Price = 10.0m
            };

            dbContext.CartItems.Add(cartItem);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(cartItem.CartItemId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cartItem.CartItemId, result.CartItemId);
        }


        [Fact]
        public async Task GetByCartIdAsync_ShouldReturnCartItems()
        {
            // Arrange
            var dbContext = GetDbContext(Guid.NewGuid().ToString());
            var repository = new CartItemRepository(dbContext);

            var cart = new ShoppingCart
            {
                CustomerId = 1,
                DateCreated = DateTime.UtcNow
            };

            var product = new Product
            {
                ProductId = 1,
                ProdCatId = 2,
                Description = "Test Product",
                Manufacturer = "Test Manufacturer",
                Stock = 10,
                BuyPrice = 5.0m,
                SellPrice = 10.0m
            };

            dbContext.Products.Add(product);
            dbContext.ShoppingCarts.Add(cart);
            await dbContext.SaveChangesAsync(); // Save ShoppingCart to generate CartId

            var cartItem1 = new CartItem
            {
                CartId = cart.CartId, // Use the generated CartId
                ProductId = product.ProductId,
                Quantity = 1,
                Price = 10.0m
            };

            var cartItem2 = new CartItem
            {
                CartId = cart.CartId, // Use the generated CartId
                ProductId = product.ProductId,
                Quantity = 2,
                Price = 20.0m
            };

            dbContext.CartItems.AddRange(cartItem1, cartItem2);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.GetByCartIdAsync(cart.CartId);

            // Assert
            Assert.Equal(2, result.Count());
        }
        
        [Fact]
        public async Task UpdateAsync_ShouldUpdateCartItem()
        {
            // Arrange
            var dbContext = GetDbContext(Guid.NewGuid().ToString());
            var repository = new CartItemRepository(dbContext);

            var product = new Product
            {
                ProductId = 1,
                ProdCatId = 2,
                Description = "Test Product",
                Manufacturer = "Test Manufacturer",
                Stock = 10,
                BuyPrice = 5.0m,
                SellPrice = 10.0m
            };

            var cart = new ShoppingCart
            {
                CustomerId = 1,
                DateCreated = DateTime.UtcNow
            };

            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = product.ProductId,
                Quantity = 1,
                Price = 10.0m
            };

            dbContext.Products.Add(product);
            dbContext.ShoppingCarts.Add(cart);
            dbContext.CartItems.Add(cartItem);
            await dbContext.SaveChangesAsync();

            // Act
            cartItem.Quantity = 5;
            await repository.UpdateAsync(cartItem);

            // Assert
            var updatedCartItem = await dbContext.CartItems.FindAsync(cartItem.CartItemId);
            Assert.Equal(5, updatedCartItem.Quantity);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCartItem()
        {
            // Arrange
            var dbContext = GetDbContext(Guid.NewGuid().ToString());
            var repository = new CartItemRepository(dbContext);

            var product = new Product
            {
                ProductId = 1,
                ProdCatId = 2,
                Description = "Test Product",
                Manufacturer = "Test Manufacturer",
                Stock = 10,
                BuyPrice = 5.0m,
                SellPrice = 10.0m
            };

            var cart = new ShoppingCart
            {
                CustomerId = 1,
                DateCreated = DateTime.UtcNow
            };

            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = product.ProductId,
                Quantity = 1,
                Price = 10.0m
            };

            dbContext.Products.Add(product);
            dbContext.ShoppingCarts.Add(cart);
            dbContext.CartItems.Add(cartItem);
            await dbContext.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(cartItem.CartItemId);

            // Assert
            var deletedCartItem = await dbContext.CartItems.FindAsync(cartItem.CartItemId);
            Assert.Null(deletedCartItem);
        }
    }