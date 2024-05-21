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

            //Assert
            Assert.True(response.CountryID != Ulid.Empty);
        }
    }
}
