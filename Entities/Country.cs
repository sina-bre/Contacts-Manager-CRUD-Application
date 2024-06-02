using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Model for Country
    /// </summary>
    public class Country
    {
        [Key]
        [StringLength(26)]
        public Ulid ID { get; set; }
        public string? Name { get; set; }
    }
}
