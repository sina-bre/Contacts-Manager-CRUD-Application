using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Person
    {
        [Key]
        [StringLength(26)]
        public Ulid ID { get; set; }
        [StringLength(50)]
        public string? PersonName { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(30)]
        public string? Gender { get; set; }
        public Ulid? CountryID { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
    }
}
