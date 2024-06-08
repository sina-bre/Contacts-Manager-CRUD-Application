using ServiceContracts.DTO.CountryDTO;
using ServiceContracts.Interfaces;

namespace Application_Tests.Helpers
{
    public class CreateCountryHelper
    {

        internal static async Task<CountryResponse> CountryCreator(ICountriesService countriesService, string CountryName)
        {
            CountryAddRequest countryAddRequest = new()
            {
                CountryName = CountryName
            };
            return await countriesService.AddCountry(countryAddRequest);
        }
    }
}
