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

        /// <summary>
        /// Returns the person object based on the given person id 
        /// </summary>
        /// <param name="personID">Person id to search</param>
        /// <returns>Matching person object</returns>
        PersonResponse? GetPersonByPersonId(Ulid? personID);

        /// <summary>
        /// Returns all persons objects that matches with the given search field and search string
        /// </summary>
        /// <param name="serachBy">Serach field to serach</param>
        /// <param name="serachString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search</returns>
        List<PersonResponse> GetFilteredPersons(string serachBy, string serachString);

    }
}
