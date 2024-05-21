using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents busuness logic for maniupulaiting Country entiy
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>Returns the country object after adding it (including newly generated )</returns>
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// return all countries from list
        /// </summary>
        /// <returns>All Countries from the list as List of CountryResponse></returns>
        List<CountryResponse> GetAllCountries();
    }
}
