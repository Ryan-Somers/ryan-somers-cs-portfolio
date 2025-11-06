using System;
using System.Collections.Generic;

namespace rsH60Store.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int ProdCatId { get; set; }

    public string Description { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public int Stock { get; set; }
    

    [PriceComparison(ErrorMessage = "Buy price must be less than the sell price.")]
    public decimal? BuyPrice { get; set; }

    [PriceComparison(ErrorMessage = "Sell price must be greater than the buy price.")]
    public decimal? SellPrice { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ProductCategory ProdCat { get; set; } = null!;
}
