using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Invoice_Book
{
    public class InvoiceBook
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("CompanyCode")]
        public int CompanyId;
        private Registered_Companies.RegisteredCompany? Company;
    }
}
