namespace rsH60Store.Models.Interfaces;

public interface ISoapServiceRepository
{
    Task<bool> ValidateCreditCardAsync(string cardNumber);
}