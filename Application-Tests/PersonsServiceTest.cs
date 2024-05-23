using Entities;
using ServiceContracts.DTO.CountryDTO;
using ServiceContracts.DTO.Enums;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Interfaces;
using Services;
using Xunit.Abstractions;


namespace Application_Tests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
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

        #region GetAllPersons

        //The GetAllPersons() should return an empty list by default
        [Fact]
        public void GetAllPersons_EpmtyList()
        {
            //Act
            List<PersonResponse> personsFromGet = _personsService.GetAllPersons();

            //Assert
            Assert.Empty(personsFromGet);
        }

        //First we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new()
            {
                CountryName = "France"
            };
            CountryAddRequest countryAddRequest2 = new()
            {
                CountryName = "Japan"
            };
            CountryAddRequest countryAddRequest3 = new()
            {
                CountryName = "Iran"
            };

            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryAddRequest3);

            PersonAddRequest? personAddRequest1 = new()
            {
                PersonName = "Person name1",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };
            PersonAddRequest? personAddRequest2 = new()
            {
                PersonName = "Person name2",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };
            PersonAddRequest? personAddRequest3 = new()
            {
                PersonName = "Person name3",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse3.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };

            List<PersonAddRequest> personRequests = new()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            List<PersonResponse> personResponseListFromAdd = new();
            foreach (PersonAddRequest personRequest in personRequests)
            {
                PersonResponse personResponse = _personsService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            //print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personResponseListFromGet = _personsService.GetAllPersons();
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse persoResponseFromGet in personResponseListFromGet)
            {
                _testOutputHelper.WriteLine(persoResponseFromGet.ToString());
            }

            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                Assert.Contains(personResponseFromAdd, personResponseListFromGet);
            }

        }
        #endregion


        #region GetFilteredPersons
        //If the search text is empty and search by is "PersonName", should return all persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new()
            {
                CountryName = "France"
            };
            CountryAddRequest countryAddRequest2 = new()
            {
                CountryName = "Japan"
            };
            CountryAddRequest countryAddRequest3 = new()
            {
                CountryName = "Iran"
            };

            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryAddRequest3);

            PersonAddRequest? personAddRequest1 = new()
            {
                PersonName = "Person name1",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };
            PersonAddRequest? personAddRequest2 = new()
            {
                PersonName = "Person name2",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };
            PersonAddRequest? personAddRequest3 = new()
            {
                PersonName = "Person name3",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse3.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };

            List<PersonAddRequest> personRequests = new()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            List<PersonResponse> personResponseListFromAdd = new();
            foreach (PersonAddRequest personRequest in personRequests)
            {
                PersonResponse personResponse = _personsService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            //print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsListFromSeacrh = _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse persoResponseFromGet in personsListFromSeacrh)
            {
                _testOutputHelper.WriteLine(persoResponseFromGet.ToString());
            }

            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                Assert.Contains(personResponseFromAdd, personsListFromSeacrh);
            }
        }


        //First we will add few persons; and then we will search based on person name with some search string. It should return the matching person
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new()
            {
                CountryName = "France"
            };
            CountryAddRequest countryAddRequest2 = new()
            {
                CountryName = "Japan"
            };
            CountryAddRequest countryAddRequest3 = new()
            {
                CountryName = "Iran"
            };

            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryAddRequest3);

            PersonAddRequest? personAddRequest1 = new()
            {
                PersonName = "Carlos",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };
            PersonAddRequest? personAddRequest2 = new()
            {
                PersonName = "Urbes",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };
            PersonAddRequest? personAddRequest3 = new()
            {
                PersonName = "Normes",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse3.CountryID,
                Gender = GenderOptions.Male,
                ReciveNewsLetter = true,
            };

            List<PersonAddRequest> personRequests = new()
            {
                personAddRequest1,
                personAddRequest2,
                personAddRequest3
            };

            List<PersonResponse> personResponseListFromAdd = new();
            foreach (PersonAddRequest personRequest in personRequests)
            {
                PersonResponse personResponse = _personsService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            //print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsListFromSeacrh = _personsService.GetFilteredPersons(nameof(Person.PersonName), "es");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse persoResponseFromGet in personsListFromSeacrh)
            {
                _testOutputHelper.WriteLine(persoResponseFromGet.ToString());
            }

            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                if (personResponseFromAdd.PersonName is not null)
                {
                    if (personResponseFromAdd.PersonName.Contains("es", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(personResponseFromAdd, personsListFromSeacrh);
                    }
                }
            }
        }
        #endregion
    }
}
