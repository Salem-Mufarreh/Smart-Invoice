using Smart_Invoice.Models.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Warehouse
{
    public class WarehouseProduct
    {
        [Key]
        public int WarehouseProductId { get; set; }

        // Foreign keys for the two related entities
        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        public virtual Warehouse? Warehouse { get; set; }
        public virtual Product? Product { get; set; }
    }
}
