using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO.CountryDTO;
using ServiceContracts.Interfaces;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonsDBContext _dbContext;

        public CountriesService(PersonsDBContext personsDBContext)
        {
            _dbContext = personsDBContext;
        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
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
            if (await _dbContext.Countries.CountAsync(temp => temp.Name == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();
            //generate CountryID
            country.ID = Ulid.NewUlid();

            //Add country object into _dbContext
            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();

            return country.ToCountryRespone();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _dbContext.Countries.Select(country => country.ToCountryRespone()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Ulid? countryID)
        {
            if (countryID is null)
                return null;

            //Country? countryResponseFromList = await _dbContext.Countries.FirstOrDefaultAsync(countryTemp => countryTemp.ID == countryID);
            Country? countryResponseFromList = await _dbContext.Countries.FindAsync(countryID);


            if (countryResponseFromList == null)
                return null;

            return countryResponseFromList.ToCountryRespone();
        }
    }
}
