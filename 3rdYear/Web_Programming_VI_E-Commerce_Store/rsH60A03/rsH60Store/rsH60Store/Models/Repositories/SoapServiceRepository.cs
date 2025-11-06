using System.Text;
using rsH60Store.Models.Interfaces;
using rsomers_H60Services.DTO;
using rsomers_H60Services.Models.Interfaces;

namespace rsH60Store.Models.Repositories;

public class SoapServiceRepository: ISoapServiceRepository
{
    private readonly HttpClient _httpClient;

        public SoapServiceRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidateCreditCardAsync(string cardNumber)
        {
            try
            {
                var content = new StringContent($"\"{cardNumber}\"", Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Order/ValidateCreditCard", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Error validating credit card: {response.ReasonPhrase}");
                }

                var result = await response.Content.ReadAsStringAsync();
                return result.Contains("valid", StringComparison.OrdinalIgnoreCase); // Check response message
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while validating the credit card.", ex);
            }
        }
    }