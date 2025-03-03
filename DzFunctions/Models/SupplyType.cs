using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DzFunctions.Models
{
    [Table("SupplyTypes")]
    public class SupplyType
    {
        [Key]
        [Column("TypeID")]
        public int TypeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string TypeName { get; set; }

        public virtual ICollection<OfficeSupply> OfficeSupplies { get; set; } = new List<OfficeSupply>();
    }
}