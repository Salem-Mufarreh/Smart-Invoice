using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Invoices
{
    [Table("ProductInvoice")]
    public class Product_Invoice : Invoice
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<InvoiceItem>? Items { get; set; }

        // Add a foreign key to the Invoice model
        [ForeignKey("Id")]
        public virtual Invoice Invoice { get; set; }
    }
}
