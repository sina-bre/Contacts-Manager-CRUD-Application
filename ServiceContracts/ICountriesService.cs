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
        /// <param name="countryAddRequest"></param>
        /// <returns></returns>
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
    }
}
