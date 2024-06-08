using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO.CountryDTO;
using ServiceContracts.Interfaces;
using Services;
namespace Application_Tests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(new PersonsDBContext(new DbContextOptionsBuilder<PersonsDBContext>().Options));
        }

        #region AddCountries
        //When CountryAddRequest is null it should ArgumentNullExeption
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is Null, it shuold throw ArgumentExeption
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new()
            {
                CountryName = null
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is dublicate, it should throw ArgumentExeption
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new()
            {
                CountryName = "Poland"
            };
            CountryAddRequest? request2 = new()
            {
                CountryName = "Poland"
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request1);
            });
        }

        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = new()
            {
                CountryName = "Canada"
            };

            //Act
            CountryResponse response = await _countriesService.AddCountry(request);
            List<CountryResponse> contriesFromGetAllCountries = await _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Ulid.Empty);
            Assert.Contains(response, contriesFromGetAllCountries);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        //The list of countries should be empty by default (before adding any countries) 
        public async Task GetAllCountries_EmptyList()
        {
            //Acts
            List<CountryResponse> countries = await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(countries);
        }

        [Fact]
        public async Task GetAllCountryDetails_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> countryRequestList = new()
            {
                new CountryAddRequest()
                {
                    CountryName = "Poland"
                },
                new CountryAddRequest()
                {
                    CountryName = "Italy"
                }
            };

            //Act
            List<CountryResponse> countriesListFromAddCountry = new() { };
            foreach (CountryAddRequest countryRequest in countryRequestList)
            {
                countriesListFromAddCountry.Add(await _countriesService.AddCountry(countryRequest))
               ;
            }

            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            //read each element from contriesListFromAddCountry
            foreach (CountryResponse expectedCountry in countriesListFromAddCountry)
            {
                Assert.Contains(expectedCountry, actualCountryResponseList);
            }
        }
        #endregion

        #region
        [Fact]
        //If we supply null as CountryID, it should returns null as CountryResponse
        public async Task GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Ulid? countryID = null;

            //Act
            CountryResponse? countryResponseFromGetMethod = await _countriesService.GetCountryByCountryID(countryID);

            Assert.Null(countryResponseFromGetMethod);
        }

        [Fact]
        //If we supply a valid country id, it should return the matching country details as CountryResponse object  
        public async Task GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new()
            {
                CountryName = "China",
            };
            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            CountryResponse? countryResponseFromGet = await _countriesService.GetCountryByCountryID(countryResponseFromAdd.CountryID);

            Assert.Equal(countryResponseFromAdd, countryResponseFromGet);
        }

        #endregion
    }
}
