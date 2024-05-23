using Entities;

namespace ServiceContracts.DTO.PersonDTO
{
    public static class PersonExtensions
    {
        /// <summary>
        /// An extension method to convert an objecct of Person class into PersonResponse class
        /// </summary>
        /// <param name="person">Person object as parameter</param>
        /// <retuens>Returns the converted PersonResponse object</retuens>
        public static double? CalculateAgeByDateOfBirth(DateTime? date)
        {
            if (date is not null)
            {
                return Math.Round((DateTime.Now - date.Value).TotalDays / 365.25);
            }
            else
            {
                return null;
            }
        }
        public static PersonResponse ToPerosnResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.ID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                ReciveNewsLetter = person.ReciveNewsLetters,
                Age = CalculateAgeByDateOfBirth(person.DateOfBirth),
            };
        }
    }
}
