using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DzFunctions.Models
{
    [Table("SalesManagers")]
    public class SalesManager
    {
        [Key]
        [Column("ManagerID")]
        public int ManagerID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}