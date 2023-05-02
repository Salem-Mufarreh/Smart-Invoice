using System.ComponentModel.DataAnnotations;

namespace Smart_Invoice.Models.Invoices
{
    public class Product_Invoice : Invoice
    {
        public InvoiceItem[]? Items { get; set; }

    }
}
