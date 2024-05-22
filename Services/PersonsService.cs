using Entities;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Interfaces;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }
        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPerosnResponse();
            personResponse.CountryName = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;

            return personResponse;

        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest is null)
                throw new ArgumentNullException(nameof(personAddRequest));

            ValidateNullParameter(personAddRequest.PersonName, nameof(personAddRequest.PersonName), "Person Name can't be empty.");
            ValidateNullParameter(personAddRequest.Email, nameof(personAddRequest.Email), "Email can't be empty.");
            ValidateNullParameter(personAddRequest.DateOfBirth, nameof(personAddRequest.DateOfBirth), "Date of Birth can't be empty.");
            ValidateNullParameter(personAddRequest.CountryID, nameof(personAddRequest.CountryID), "Country ID can't be empty.");
            ValidateNullParameter(personAddRequest.Address, nameof(personAddRequest.Address), "Address can't be empty.");

            //convert personAddrequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonID
            person.ID = Ulid.NewUlid();

            //add person object to persons list
            _persons.Add(person);

            //convert the Person object into PersonResponse type
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        public static void ValidateNullParameter(object? parameter, string parameterName, string message)
        {
            if (parameter is null)
            {
                throw new ArgumentException(message, parameterName);
            }
        }
    }
}
