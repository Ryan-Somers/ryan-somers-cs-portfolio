using System.ComponentModel.DataAnnotations;
using rsH60Store.Models.Interfaces;
using rsH60Store.Models.Interfaces;

public class ValidCreditCardAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string creditCardNumber || string.IsNullOrWhiteSpace(creditCardNumber))
        {
            return new ValidationResult("Credit card number is required.");
        }

        try
        {
            // Resolve the service
            var soapServiceRepository = (ISoapServiceRepository)validationContext
                .GetService(typeof(ISoapServiceRepository));
            
            if (soapServiceRepository == null)
            {
                throw new InvalidOperationException("Unable to resolve ISoapServiceRepository.");
            }

            // Validate the credit card
            var isValid = soapServiceRepository.ValidateCreditCardAsync(creditCardNumber).Result;
            return isValid
                ? ValidationResult.Success
                : new ValidationResult("Invalid credit card number.");
        }
        catch (Exception ex)
        {
            return new ValidationResult($"Product of last 2 digits must be multiple of 2. Must be 12-16 characters.");
        }
    }
}