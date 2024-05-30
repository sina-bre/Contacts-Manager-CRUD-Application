using Entities;
using ServiceContracts.DTO.Enums;
using System.ComponentModel.DataAnnotations;
namespace ServiceContracts.DTO.PersonDTO
{
    /// <summary>
    /// Acts as a DTO for inserting a new person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "{0} can't be empty")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "{0} can't be empty")]
        [EmailAddress(ErrorMessage = "{0} value should be a valid email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "{0} can't be empty")]
        public GenderOptions Gender { get; set; }

        [Required(ErrorMessage = "Please selcet a country")]
        public Ulid? CountryID { get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
