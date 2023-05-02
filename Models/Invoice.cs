using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required]
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [Required]
        [StringLength(50)]
        public int InvoiceNumber { get; set; }
  
       
        [Required]
        public DateTime InvoiceDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public Double TotalAmount { get; set; }
        [Required]
        [StringLength(50)]
        public string CurrencyCode { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        [Required]
        [StringLength(50)]
        public DateTime PaymentDate { get; set; }
        /* the exchange rate takes the USD as the base currancy */
        [Required]
        public Double ExchangeRate { get; set; }
        /* effective date is the date when the currancy exchange happend */ 
        [Required]
        public DateTime EffectiveDate { get; set; }
        [Required]
        public Double TaxPercentage { get; set; }
        [Required]
        public Double TaxTotal { get; set; }






    }
}
