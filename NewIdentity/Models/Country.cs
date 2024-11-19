using System.ComponentModel.DataAnnotations;

namespace NewIdentity.Models
{
    public class Country

    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}
