using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: countryAddRequest parameters can't be null
            if (countryAddRequest is null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validation: countryName can't be null
            if (countryAddRequest.CountryName is null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation: Dublication of countryName does not allowed
            if (_countries.Where(temp => temp.Name == countryAddRequest.CountryName).Any())
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();
            //generate CountryID
            country.Id = Ulid.NewUlid();

            //Add country object into _countries
            _countries.Add(country);

            return country.ToCountryRespone();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryRespone()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Ulid? countryID)
        {
            if (countryID is null)
                return null;

            Country? countryResponseFromList = _countries.FirstOrDefault(countryTemp => countryTemp.Id == countryID);

            if (countryResponseFromList is null)
                return null;

            return countryResponseFromList.ToCountryRespone();
        }
    }
}
