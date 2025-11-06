using System.Threading.Tasks;
using rsomers_H60Services.CheckCreditCard;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Models.Repositories;


public class CreditCardService: ICheckCreditCard
{
    private readonly CheckCreditCardSoapClient _soapClient;

    public CreditCardService()
    {
        _soapClient = new CheckCreditCardSoapClient(CheckCreditCardSoapClient.EndpointConfiguration.CheckCreditCardSoap);
    }

    public async Task<bool> ValidateCreditCardAsync(string cardNumber)
    {
        try
        {
            int validationResult = await _soapClient.CreditCardCheckAsync(cardNumber);
            return validationResult == 0; // 0 is valid, 1 is invalid
        }
        catch (Exception ex)
        {
            // Log exception or handle accordingly
            Console.WriteLine($"Error validating credit card: {ex.Message}");
            return false;
        }
        finally
        {
            if (_soapClient.State == System.ServiceModel.CommunicationState.Opened)
            {
                await _soapClient.CloseAsync();
            }
        }
    }
}