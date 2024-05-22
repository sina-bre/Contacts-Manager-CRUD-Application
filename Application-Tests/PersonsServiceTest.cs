using ServiceContracts.DTO.CountryDTO;
using ServiceContracts.DTO.Enums;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Interfaces;
using Services;


namespace Application_Tests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        public PersonsServiceTest()
        {
            _personsService = new PersonsService();
            _countriesService = new CountriesService();
        }

        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumnetNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as PersonName, it should throw ArgumnetException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                PersonName = null,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as Email, it should throw ArgumnetException
        [Fact]
        public void AddPerson_EamilIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                Email = null,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as Address, it should throw ArgumnetException
        [Fact]
        public void AddPerson_AddressIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                Address = null,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as DateOfBirth, it should throw ArgumnetException
        [Fact]
        public void AddPerson_DateOfBirthIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                DateOfBirth = null,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as CountryID, it should throw ArgumnetException
        [Fact]
        public void AddPerson_CountryIDIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                CountryID = null,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                PersonName = "Person name",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = Ulid.NewUlid(),
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };

            //Act
            PersonResponse personResponseFromAdd = _personsService.AddPerson(personAddRequest);

            List<PersonResponse> personsList = _personsService.GetAllPersons();

            //Assert
            Assert.True(personResponseFromAdd.PersonID != Ulid.Empty);

            Assert.Contains(personResponseFromAdd, personsList);
        }

        #endregion

        #region GetPersonByPersonID
        //if we supply null as PersonID, it should return null as PersonResponse

        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Ulid? personID = null;

            //Act
            PersonResponse? personResponseFromGet = _personsService.GetPersonByPersonId(personID);

            //Assert
            Assert.Null(personResponseFromGet);
        }

        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public void GetPersonByPersonID_WitchPersonID()
        {
            //Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Iran"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);

            //Act
            PersonAddRequest personRequest = new PersonAddRequest()
            {
                PersonName = "Person name",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };

            PersonResponse personResponseFromAdd = _personsService.AddPerson(personRequest);

            PersonResponse? personResponseFromGet = _personsService.GetPersonByPersonId(personResponseFromAdd.PersonID);

            //Assert
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
        }
        #endregion
    }
}
