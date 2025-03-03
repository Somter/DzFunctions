using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DzFunctions.Models
{
    [Table("BuyerCompanies")]
    public class BuyerCompany
    {
        [Key]
        [Column("BuyerID")]
        public int BuyerID { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}