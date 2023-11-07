using Newtonsoft.Json;
using Smart_Invoice.Models.Registered_Companies;
using Smart_Invoice.Utility;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Invoices
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty("Invoice_Id")]
        [DefaultValue("")]
        public string? Invoice_Id { get; set; }
        [Required]
        [JsonProperty("Invoice_Type")]
        [DefaultValue(SD.InvoiceProduct)]
        public string? Invoice_Type { get; set; }
        [Required]
        [JsonProperty("Invoice_Number")]
        [DefaultValue("0")]
        public string? Invoice_Number { get; set; }
        [Required]
        [JsonProperty("Invoice_Date")]
        [DefaultValue("")]
        public string? Invoice_Date { get; set; }
        [Required]
        [JsonProperty("Subtotal")]
        [DefaultValue(0.0)]
        public double? Subtotal { get; set; }
        [Required]
        [JsonProperty("Tax")]
        [DefaultValue(0.0)]
        public double? Tax { get; set; }
        [Required]
        [JsonProperty("Total")]
        [DefaultValue(0.0)]
        public double? Total { get; set; }
        [Required]
        [ForeignKey("CompanyId")]
        public Company? CompanyID { get; set; }

        public Company? Company { get; set; }

        [ForeignKey("CompanyCode")]
        public long? CompanyCode { get; set; }
        public virtual RegisteredCompany? RegisteredCompany { get; set; }
        [Column("Currancy_Code")]
        public string ? CurrancyCode { get; set; }
    }
}
