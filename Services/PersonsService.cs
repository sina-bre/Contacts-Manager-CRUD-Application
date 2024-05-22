using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Interfaces;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            throw new NotImplementedException();
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }
    }
}
