using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Invoices
{
    public class InvoiceItem
    {
        
        public int? InvoiceItemId { get; set; }
        
        [Required]
        public string? Name { get; set; }
        [Required]
        public int? Quantity { get; set; }
        public string? Unit { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public double Total { get; set; }


        public int ProductInvoiceId { get; set; }
        [ForeignKey("ProductInvoiceId")]
        public virtual Product_Invoice ProductInvoice { get; set; }
    }
}
