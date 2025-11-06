namespace rsH60Customer.Models.Interfaces;

public interface ISoapServiceRepository
{
    Task<decimal> CalculateTaxesAsync(decimal amount, string province);
    Task<bool> ValidateCreditCardAsync(string cardNumber);
}