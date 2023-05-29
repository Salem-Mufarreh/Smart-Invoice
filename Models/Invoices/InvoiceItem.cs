using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Invoices
{
    public class InvoiceItem
    {
        
        public int? InvoiceItemId { get; set; }
        
        [Required]
        [JsonProperty("Name")]
        [DefaultValue("")]
        public string? Name { get; set; }
        [Required]
        [JsonProperty("Quantity")]
        [DefaultValue(0)]
        public int? Quantity { get; set; }
        [JsonProperty("Unit")]
        [DefaultValue("BOX")]
        public string? Unit { get; set; }
        [Required]
        [JsonProperty("UnitPrice")]
        [DefaultValue(0.0)]
        public double UnitPrice { get; set; }
        [Required]
        [JsonProperty("Total")]
        [DefaultValue(0.0)]
        public double Total { get; set; }


        public int ProductInvoiceId { get; set; }
        [ForeignKey("ProductInvoiceId")]
        public virtual Product_Invoice ProductInvoice { get; set; }
    }
}
