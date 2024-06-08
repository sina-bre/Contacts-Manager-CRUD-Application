using Application_Tests.Helpers;
using Entities;
using Microsoft.EntityFrameworkCore;
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
            _countriesService = new CountriesService(new PersonsDBContext(new DbContextOptionsBuilder<PersonsDBContext>().Options));
            _personsService = new PersonsService(new PersonsDBContext(new DbContextOptionsBuilder<PersonsDBContext>().Options), _countriesService);
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumnetNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as PersonName, it should throw ArgumnetException
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                PersonName = null,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as Email, it should throw ArgumnetException
        [Fact]
        public async Task AddPerson_EamilIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                Email = null,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as Address, it should throw ArgumnetException
        [Fact]
        public async Task AddPerson_AddressIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                Address = null,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as DateOfBirth, it should throw ArgumnetException
        [Fact]
        public async Task AddPerson_DateOfBirthIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                DateOfBirth = null,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply null value as CountryID, it should throw ArgumnetException
        [Fact]
        public async Task AddPerson_CountryIDIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new()
            {
                CountryID = null,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest);
            });
        }

        //When we supply proper person details, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
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
                ReceiveNewsLetters = true,
            };

            //Act
            PersonResponse personResponseFromAdd = await _personsService.AddPerson(personAddRequest);

            List<PersonResponse> personsList = await _personsService.GetAllPersons();

            //Assert
            Assert.True(personResponseFromAdd.PersonID != Ulid.Empty);

            Assert.Contains(personResponseFromAdd, personsList);
        }

        #endregion

        #region GetPersonByPersonID
        //if we supply null as PersonID, it should return null as PersonResponse

        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Ulid? personID = null;

            //Act
            PersonResponse? personResponseFromGet = await _personsService.GetPersonByPersonId(personID);

            //Assert
            Assert.Null(personResponseFromGet);
        }

        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonID_WitchPersonID()
        {
            //Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Iran"
            };
            CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

            //Act
            PersonAddRequest personRequest = new PersonAddRequest()
            {
                PersonName = "Person name",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };

            PersonResponse personResponseFromAdd = await _personsService.AddPerson(personRequest);

            PersonResponse? personResponseFromGet = await _personsService.GetPersonByPersonId(personResponseFromAdd.PersonID);

            //Assert
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
        }
        #endregion

        #region GetAllPersons

        //The GetAllPersons() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_EpmtyList()
        {
            //Act
            List<PersonResponse> personsFromGet = await _personsService.GetAllPersons();

            //Assert
            Assert.Empty(personsFromGet);
        }

        //First we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
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

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);
            CountryResponse countryResponse3 = await _countriesService.AddCountry(countryAddRequest3);

            PersonAddRequest? personAddRequest1 = new()
            {
                PersonName = "Person name1",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest? personAddRequest2 = new()
            {
                PersonName = "Person name2",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest? personAddRequest3 = new()
            {
                PersonName = "Person name3",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse3.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
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
                PersonResponse personResponse = await _personsService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            //print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personResponseListFromGet = await _personsService.GetAllPersons();
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
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryResponse countryResponse1 = await CreateCountryHelper.CountryCreator(_countriesService, "Italy");
            CountryResponse countryResponse2 = await CreateCountryHelper.CountryCreator(_countriesService, "USA");
            CountryResponse countryResponse3 = await CreateCountryHelper.CountryCreator(_countriesService, "Iran");

            PersonAddRequest? personAddRequest1 = new()
            {
                PersonName = "Person name1",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest? personAddRequest2 = new()
            {
                PersonName = "Person name2",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest? personAddRequest3 = new()
            {
                PersonName = "Person name3",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse3.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
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
                PersonResponse personResponse = await _personsService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            //print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsListFromSeacrh = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

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
        public async Task GetFilteredPersons_SearchByPersonName()
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

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);
            CountryResponse countryResponse3 = await _countriesService.AddCountry(countryAddRequest3);

            PersonAddRequest? personAddRequest1 = new()
            {
                PersonName = "Carlos",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest? personAddRequest2 = new()
            {
                PersonName = "Urbes",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest? personAddRequest3 = new()
            {
                PersonName = "Normes",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse3.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
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
                PersonResponse personResponse = await _personsService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            //print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            //Act
            List<PersonResponse> personsListFromSeacrh = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "es");

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

        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName 
        [Fact]
        public async Task GetSortedPersons()
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

            CountryResponse countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);
            CountryResponse countryResponse3 = await _countriesService.AddCountry(countryAddRequest3);

            PersonAddRequest? personAddRequest1 = new()
            {
                PersonName = "Carlos",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest? personAddRequest2 = new()
            {
                PersonName = "Urbes",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-08-02"),
                Address = "sample address",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest? personAddRequest3 = new()
            {
                PersonName = "Normes",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-04-03"),
                Address = "sample address",
                CountryID = countryResponse3.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
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
                PersonResponse personResponse = await _personsService.AddPerson(personRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            personResponseListFromAdd = personResponseListFromAdd.OrderBy(temp => temp.DateOfBirth).ToList();

            //print personResponseListFromAdd
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }
            List<PersonResponse> allPersons = await _personsService.GetAllPersons();
            //Act
            List<PersonResponse> personsListFromSort = await _personsService.GetSortedPersons(allPersons, nameof(Person.DateOfBirth), SortOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse persoResponseFromSort in personsListFromSort)
            {
                _testOutputHelper.WriteLine(persoResponseFromSort.ToString());
            }


            for (int i = 0; i < personResponseListFromAdd.Count; i++)
            {
                Assert.Equal(personResponseListFromAdd[i], personsListFromSort[i]);
            }
        }
        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullExeption
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
           {
               //Act
               PersonResponse personResponse = await _personsService.UpdatePerson(personUpdateRequest);
           });
        }

        //When we supply invalid PersonID, it should return throw argument exception
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new()
            {
                PersonID = Ulid.NewUlid()
            };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                PersonResponse personResponse = await _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        [Theory]
        [InlineData(null, "PersonName")]
        [InlineData(null, "Email")]
        [InlineData(null, "CountryID")]
        [InlineData(null, "DateOfBirth")]
        [InlineData(null, "Address")]
        public async Task UpdatePerson_PropertyIsNull(string? nullValue, string propertyName)
        {
            // Arrange
            CountryResponse countryResponse = await CreateCountryHelper.CountryCreator(_countriesService, "Italy");
            PersonAddRequest personAddRequest = new()
            {
                PersonName = "Carlos",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonResponse personResponseFromAdd = await _personsService.AddPerson(personAddRequest);
            PersonUpdateRequest personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();
            typeof(PersonUpdateRequest).GetProperty(propertyName)?.SetValue(personUpdateRequest, nullValue);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                PersonResponse personResponse = await _personsService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation()
        {
            // Arrange
            CountryResponse countryResponse = await CreateCountryHelper.CountryCreator(_countriesService, "Iran");
            PersonAddRequest personAddRequest = new()
            {
                PersonName = "Carlos",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonResponse personResponseFromAdd = await _personsService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();
            personAddRequest.PersonName = "Parmida";
            personAddRequest.Email = "Upadtedperson@example.com";

            //Act
            PersonResponse personResponseFromUpdate = await _personsService.UpdatePerson(personUpdateRequest);

            PersonResponse? personResponseFromGet = await _personsService.GetPersonByPersonId(personResponseFromUpdate.PersonID);

            //Assert
            Assert.Equal(personResponseFromGet, personResponseFromUpdate);
        }
        #endregion

        #region DeletePerson

        //if you supply an valid PersonID, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryResponse countryResponse = await CreateCountryHelper.CountryCreator(_countriesService, "Italy");

            PersonAddRequest personAddRequest = new()
            {
                PersonName = "Carlos",
                Email = "person@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Address = "sample address",
                CountryID = countryResponse.CountryID,
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
            };
            PersonResponse personResponseFromAdd = await _personsService.AddPerson(personAddRequest);

            //Act
            bool isDeleted = await _personsService.DeletePerson(personResponseFromAdd.PersonID);

            Assert.True(isDeleted);
        }

        //if you supply an invalid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted = await _personsService.DeletePerson(Ulid.NewUlid());

            Assert.False(isDeleted);
        }
        #endregion
    }
}
