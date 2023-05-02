using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Invoices
{
    public class InvoiceItem
    {
        [Key]
        public int id { get; set; }
        public int? InvoiceItemId { get; set; }
        [Required]
        [ForeignKey("Id")]
        public Invoice? InvoiceId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int? Quantity { get; set; }
        public string? Unit { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public double Total { get; set; }



    }
}
