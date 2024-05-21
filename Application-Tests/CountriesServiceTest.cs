using ServiceContracts;
using ServiceContracts.DTO;
using Services;
namespace Application_Tests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountries
        //When CountryAddRequest is null it should ArgumentNullExeption
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is Null, it shuold throw ArgumentExeption
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new()
            {
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When the CountryName is dublicate, it should throw ArgumentExeption
        [Fact]
        public void AddCountry_DuplicateCountryName()
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
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request1);
            });
        }

        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = new()
            {
                CountryName = "Canada"
            };

            //Act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> contriesFromGetAllCountries = _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Ulid.Empty);
            Assert.Contains(response, contriesFromGetAllCountries);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        //The list of countries should be empty by default (before adding any countries) 
        public void GetAllCountries_EmptyList()
        {
            //Acts
            List<CountryResponse> countries = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(countries);
        }

        [Fact]
        public void GetAllCountryDetails_AddFewCountries()
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
                countriesListFromAddCountry.Add(_countriesService.AddCountry(countryRequest))
               ;
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

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
        public void GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Ulid? countryID = null;

            //Act
            CountryResponse? countryResponseFromGetMethod = _countriesService.GetCountryByCountryID(countryID);

            Assert.Null(countryResponseFromGetMethod);
        }

        [Fact]
        //If we supply a valid country id, it should return the matching country details as CountryResponse object  
        public void GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new()
            {
                CountryName = "China",
            };
            CountryResponse countryResponseFromAdd = _countriesService.AddCountry(countryAddRequest);

            CountryResponse? countryResponseFromGet = _countriesService.GetCountryByCountryID(countryResponseFromAdd.CountryID);

            Assert.Equal(countryResponseFromAdd, countryResponseFromGet);
        }

        #endregion
    }
}
