using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace Smart_Invoice.Models.Products
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public double? CostPrice {  get; set; }
        public long? CategoryId { get; set;}
        public string? SKU { get; set; } // barcode, item coce ,....
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
      

       // Navigation property
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [ForeignKey(name: "SalesInvoiceId")]
        [Column("SalesInvoiceId")]
        public int? SalesInvoiceId { get; set; }
    }
}
