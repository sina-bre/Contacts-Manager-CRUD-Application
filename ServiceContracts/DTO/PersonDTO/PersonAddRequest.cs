using Entities;
using ServiceContracts.DTO.Enums;

namespace ServiceContracts.DTO.PersonDTO
{
    /// <summary>
    /// Acts as a DTO for inserting a new person
    /// </summary>
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions Gender { get; set; }
        public Ulid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReciveNewsLetter { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                Name = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReciveNewsLetters = ReciveNewsLetter
            };
        }
    }
}
