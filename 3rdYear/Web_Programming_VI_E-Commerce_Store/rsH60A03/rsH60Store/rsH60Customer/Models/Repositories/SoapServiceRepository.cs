using System.Text;
using rsH60Customer.DTO;
using rsH60Customer.Models.Interfaces;

namespace rsH60Customer.Models.Repositories;

public class SoapServiceRepository: ISoapServiceRepository
{
    private readonly HttpClient _httpClient;

        public SoapServiceRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> CalculateTaxesAsync(decimal amount, string province)
        {
            try
            {
                var taxRequest = new TaxRequestDto
                {
                    Amount = amount,
                    Province = province
                };

                var response = await _httpClient.PostAsJsonAsync("/api/Order/CalculateTaxes", taxRequest);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Error calculating taxes: {response.ReasonPhrase}");
                }

                var result = await response.Content.ReadFromJsonAsync<TaxResponseDto>();
                return result?.Taxes ?? 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while calculating taxes.", ex);
            }
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