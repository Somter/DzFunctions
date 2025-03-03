using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DzFunctions.Models
{
    [Table("Sales")]
    public class Sale
    {
        [Key]
        [Column("SaleID")]
        public int SaleID { get; set; }

        [Required]
        public int SupplyID { get; set; }

        [Required]
        public int ManagerID { get; set; }

        [Required]
        public int BuyerID { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public int QuantitySold { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SalePrice { get; set; }

        [ForeignKey(nameof(SupplyID))]
        public OfficeSupply OfficeSupply { get; set; }

        [ForeignKey(nameof(ManagerID))]
        public SalesManager SalesManager { get; set; }

        [ForeignKey(nameof(BuyerID))]
        public BuyerCompany BuyerCompany { get; set; }
    }
}