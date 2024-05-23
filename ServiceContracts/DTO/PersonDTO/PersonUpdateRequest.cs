using Entities;
using ServiceContracts.DTO.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.PersonDTO
{
    /// <summary>
    /// Represents the DTO class that contains the person details to update
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "{0} can't be empty")]
        public Ulid PersonID { get; set; }

        [Required(ErrorMessage = "{0} can't be empty")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "{0} can't be empty")]
        [EmailAddress(ErrorMessage = "{0} value should be a valid email")]
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
                ID = PersonID,
                PersonName = PersonName,
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
