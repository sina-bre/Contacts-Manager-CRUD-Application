using ServiceContracts.DTO.Enums;
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
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Retursn all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the given person id 
        /// </summary>
        /// <param name="personID">Person id to search</param>
        /// <returns>Matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonId(Ulid? personID);

        /// <summary>
        /// Returns all persons objects that matches with the given search field and search string
        /// </summary>
        /// <param name="serachBy">Serach field to serach</param>
        /// <param name="serachString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string serachBy, string? serachString);

        /// <summary>
        /// Returns sorted list of persons 
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of the property (key), based on wich the persons should be stored </param>
        /// <param name="sortOrder">ASC DESC</param>
        /// <returns>Returns sorted persons as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Update the specified person details  based on the given PersonID
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, including person id</param>
        /// <returns>Returns the PersonResponse object after update</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes a person based on the given person id
        /// </summary>
        /// <param name="personID">
        /// PersonID to delete</param>
        /// <returns>Returns true, if the deletion is successful otherwise returns false</returns>
        Task<bool> DeletePerson(Ulid? personID);

        /// <summary>
        /// Returns persons as CSV
        /// </summary>
        /// <returns>
        /// Returns the memory stream with CSV data
        /// </returns>
        Task<MemoryStream> GetPersonsCSV();

    }
}
