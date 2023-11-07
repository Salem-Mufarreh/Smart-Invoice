using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Invoices
{
    [Serializable]
    [Table("ProductInvoice")]
    public class Product_Invoice : Invoice
    {
        [Key]
        public int Id { get; set; }
        public virtual List<InvoiceItem>? Items { get; set; }

       
    }
}
