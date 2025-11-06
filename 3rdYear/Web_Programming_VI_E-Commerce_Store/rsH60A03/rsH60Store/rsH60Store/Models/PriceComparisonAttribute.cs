namespace rsH60Store.Models;

using System.ComponentModel.DataAnnotations;

public class PriceComparisonAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var model = (Product)validationContext.ObjectInstance;

        // Check if both prices are set
        if (model.BuyPrice.HasValue && model.SellPrice.HasValue)
        {
            if (model.SellPrice < model.BuyPrice)
            {
                return new ValidationResult("Sell price cannot be lower than buy price.");
            }
        }

        return ValidationResult.Success;
    }
}