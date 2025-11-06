using Microsoft.EntityFrameworkCore;
using Moq;
using rsomers_H60Services.Models;
using rsomers_H60Services.Models.Interfaces;
using rsomers_H60Services.Models.Repositories;

namespace TestShoppingCart;

public class ShoppingCartTests
{
    [Fact]
    public async Task AddAsync_ShouldAddNewShoppingCart()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServicesDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use unique database for isolation
            .Options;

        await using var context = new ServicesDBContext(options);
        var repository = new ShoppingCartRepository(context);
        var shoppingCart = new ShoppingCart
        {
            CartId = 1,
            CustomerId = 100,
            DateCreated = DateTime.UtcNow
        };

        // Act
        await repository.AddAsync(shoppingCart);

        // Assert
        var addedCart = await context.ShoppingCarts.FindAsync(1);
        Assert.NotNull(addedCart);
        Assert.Equal(1, addedCart.CartId);
        Assert.Equal(100, addedCart.CustomerId);
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnShoppingCart_WhenIdExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServicesDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for this test
            .Options;

        await using var context = new ServicesDBContext(options);
        var repository = new ShoppingCartRepository(context);

        // Seed data
        var shoppingCart = new ShoppingCart
        {
            CartId = 1,
            CustomerId = 100,
            DateCreated = DateTime.UtcNow
        };
        await context.ShoppingCarts.AddAsync(shoppingCart);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.CartId);
        Assert.Equal(100, result.CustomerId);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServicesDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServicesDBContext(options);
        var repository = new ShoppingCartRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999); // ID does not exist

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingShoppingCart()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServicesDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServicesDBContext(options);
        var repository = new ShoppingCartRepository(context);

        // Seed data
        var shoppingCart = new ShoppingCart
        {
            CartId = 1,
            CustomerId = 100,
            DateCreated = DateTime.UtcNow
        };
        await context.ShoppingCarts.AddAsync(shoppingCart);
        await context.SaveChangesAsync();

        // Act
        shoppingCart.CustomerId = 200; // Update customer ID
        await repository.UpdateAsync(shoppingCart);

        // Assert
        var updatedCart = await context.ShoppingCarts.FindAsync(1);
        Assert.NotNull(updatedCart);
        Assert.Equal(200, updatedCart.CustomerId); // Verify update
    }
    
    [Fact]
    public async Task DeleteShoppingCart_ShouldDeleteCartAndReturnNoItems()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServicesDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServicesDBContext(options);
        var repository = new ShoppingCartRepository(context);

        // Add a shopping cart and related items
        var cart = new ShoppingCart
        {
            CustomerId = 1,
            DateCreated = DateTime.UtcNow
        };
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(cart.CartId);

        // Assert
        var deletedCart = await context.ShoppingCarts.FindAsync(cart.CartId);
        Assert.Null(deletedCart); // Ensure the cart is deleted
    }

}