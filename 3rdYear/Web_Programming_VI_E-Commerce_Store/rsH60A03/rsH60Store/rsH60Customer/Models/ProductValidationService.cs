namespace rsH60Customer.Models;

public class ProductValidationService
{
    public void ValidatePrices(decimal? buyPrice, decimal? sellPrice)
    {
        if (buyPrice < 0 || sellPrice < 0)
        {
            throw new ArgumentException("Prices must be non-negative.");
        }

        if (sellPrice < buyPrice)
        {
            throw new ArgumentException("Sell price must not be less than buy price.");
        }
    }

    public decimal RoundToTwoDecimals(decimal value)
    {
        return Math.Round(value, 2);
    }

    public decimal FormatPrice(decimal price)
    {
        // Ensures price has at most 2 decimals
        return RoundToTwoDecimals(price);
    }
}
