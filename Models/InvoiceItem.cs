using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models
{
    public class InvoiceItem
    {
        [Key]
        public int InvoiceItemId { get; set; }
        [Required]
        [ForeignKey("Id")]
        public Invoice InvoiceId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Double UnitPrice { get; set; }
        [Required]
        public Double LineTotal { get; set; }
        [Required]
        public string ServiceType { get; set; }



    }
}
