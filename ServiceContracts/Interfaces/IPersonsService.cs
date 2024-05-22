using ServiceContracts.DTO.PersonDTO;

namespace ServiceContracts.Interfaces
{
    /// <summary>
    /// Represents business logic for manipulating Person Entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new person into the list od persons
        /// </summary>
        /// <param name="personAddRequest">person to add</param>
        /// <returns>Returns the same person details, along with newly generated PersonID</returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Retursn all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse</returns>
        List<PersonResponse> GetAllPersons();
    }
}
