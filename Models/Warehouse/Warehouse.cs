using Smart_Invoice.Models.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Warehouse
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }
        [Required]
        public string? WarehouseName { get; set; }
        [Required]
        public string? WarehouseCode { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public int? Capacity { get; set; }
        public int? AvailableSpace { get; set; }
        public double? OccupancyRate { get;set; }
        public string? Status { get; set; }

        public virtual ICollection<WarehouseProduct>? WarehouseProducts { get; set; }



    }
}
