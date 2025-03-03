using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DzFunctions.Models
{
    [Table("OfficeSupplies")]
    public class OfficeSupply
    {
        [Key]
        [Column("SupplyID")]
        public int SupplyID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int SupplyTypeID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitCost { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [ForeignKey(nameof(SupplyTypeID))]
        public SupplyType SupplyType { get; set; }
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}