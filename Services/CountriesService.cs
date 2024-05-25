using Entities;
using ServiceContracts.DTO.CountryDTO;
using ServiceContracts.Interfaces;
using System.Text.Json;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string relativePath = Path.Combine(baseDirectory, "mockData", "Country.json");
                string fullPath = Path.GetFullPath(relativePath);
                string jsonString = File.ReadAllText(fullPath);

                List<Country>? countries = JsonSerializer.Deserialize<List<Country>>(jsonString);

                if (countries is not null)
                    _countries.AddRange(countries);
            }
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
            country.ID = Ulid.NewUlid();

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

            Country? countryResponseFromList = _countries.FirstOrDefault(countryTemp => countryTemp.ID == countryID);

            if (countryResponseFromList is null)
                return null;

            return countryResponseFromList.ToCountryRespone();
        }
    }
}
