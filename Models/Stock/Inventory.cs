using Smart_Invoice.Models.Warehouse;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Stock
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        [Required]
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        [Required]
        public string? SKU { get; set; }
        
        public int ProductCount { get; set; }
        [Required]
        [ForeignKey("WarehouseId")]
        public int WarehouseId { get; set; } // make foreign key
        public DateTime? PurchaseDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        [Required]
        public double? SellingPrice { get; set; }
        public string? Notes { get; set; }
        public virtual Warehouse.Warehouse? Warehouse { get; set; }
    }
}
