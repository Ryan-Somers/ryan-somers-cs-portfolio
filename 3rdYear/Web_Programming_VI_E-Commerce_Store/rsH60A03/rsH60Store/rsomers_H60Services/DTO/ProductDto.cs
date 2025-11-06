namespace rsomers_H60Services.DTO;

public class ProductDto
{
    public int ProductId { get; set; }
    public int ProdCatId { get; set; }

    public string Description { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public int Stock { get; set; }

    public decimal BuyPrice { get; set; }

    public decimal SellPrice { get; set; }

    public string? ImageUrl { get; set; }

    
}