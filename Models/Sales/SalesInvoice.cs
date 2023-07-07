using Smart_Invoice.Models.Invoices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Sales
{
    public class SalesInvoice
    {
        [Key]
        public int Id { get; set; }

        public Customer? Customer { get; set; }
        public int Invoice_number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public List<InvoiceItem>? Products { get; set; }
        public double Discount { get; set; }
        public double SubTotal { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
        public double AmountPaid { get; set;}
        public double BalanceDue { get; set;}
        public string? Notes { get; set; }
    }
}
