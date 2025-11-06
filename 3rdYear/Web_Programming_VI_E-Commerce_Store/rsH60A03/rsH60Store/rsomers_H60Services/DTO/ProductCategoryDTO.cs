namespace rsomers_H60Services.DTO;

public class ProductCategoryDTO
{
    public int CategoryId { get; set; }
    public string ProdCat { get; set; }
    public List<ProductInCategoryDto> Products { get; set; }
}