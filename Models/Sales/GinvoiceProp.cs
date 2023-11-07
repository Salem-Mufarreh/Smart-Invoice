using Smart_Invoice.Models.Invoices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.Build.Framework;
using Smart_Invoice.Models.Products;

namespace Smart_Invoice.Models.Sales
{
    public class GinvoiceProp
    {
        public int Id { get; set; }

        public string? Name { get; set; }
       
        public int? Quantity { get; set; }
       
        public string? Unit { get; set; }
       
        public double UnitPrice { get; set; }
       
        public double Total { get; set; }
        [ForeignKey("ProductId")]
        public int? productId { get; set; }
        public virtual Product? Product { get; set; }

        public double Discount { get; set; }

        [ForeignKey("SalesInvoiceId")]
        public int? SalesInvoiceId { get; set; }
        public virtual SalesInvoice? Invoice { get;}


    }
}
