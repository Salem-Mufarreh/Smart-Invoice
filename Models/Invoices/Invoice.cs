using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Invoices
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public string? Invoice_Id { get; set; }
        [Required]
        public string? Invoice_Type { get; set; }
        [Required]
        public string? Invoice_Number { get; set; }
        [Required]
        public string? Invoice_Date { get; set; }
        [Required]
        public double? Subtotal { get; set; }
        [Required]
        public double? Tax { get; set; }
        [Required]
        public double? Total { get; set; }
        [Required]
        [ForeignKey("CompanyId")]
        public Company? CompanyID { get; set; }

        public Company? Company { get; set; }
    }
}
