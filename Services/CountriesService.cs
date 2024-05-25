using Entities;
using ServiceContracts.DTO.CountryDTO;
using ServiceContracts.Interfaces;
using System.Reflection;
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
                throw new ArgumentException();
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string? assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

                if (assemblyDirectory is null)
                    throw new ArgumentNullException();
                DirectoryInfo? projectDirectory = Directory.GetParent(assemblyDirectory)?.Parent?.Parent?.Parent;
                if (projectDirectory is null)
                {
                    throw new DirectoryNotFoundException($"Project directory was not found.");
                }
                string filePath = Path.Combine(projectDirectory.FullName, "Services", "mockData", "Countries.json");
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"The file '{filePath}' was not found.");
                }
                string json = File.ReadAllText(filePath);
                List<Country>? countries = JsonSerializer.Deserialize<List<Country>>(json);

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
