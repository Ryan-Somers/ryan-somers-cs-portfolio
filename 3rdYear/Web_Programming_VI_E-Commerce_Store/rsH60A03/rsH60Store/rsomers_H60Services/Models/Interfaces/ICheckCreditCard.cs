namespace rsomers_H60Services.Models.Interfaces;

public interface ICheckCreditCard
{ 
    Task<bool> ValidateCreditCardAsync(string cardNumber);
}