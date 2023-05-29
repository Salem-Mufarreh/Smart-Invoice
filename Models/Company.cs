using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        public string? Company_Name { get; set; }
        public string? Company_Name_Normilized { get; set; }
        public string? Company_Name_English { get; set; }
        [Required]
        [JsonProperty("Address")]
        [DefaultValue("")]
        public string? Address { get; set; }
        [JsonProperty("Company_License_Registration_Number")]
        [DefaultValue("")]
        public string? Company_License_Registration_Number { get; set; }
        [Required]
        [JsonProperty("Phone")]
        [DefaultValue("")]
        public string? Phone { get; set; }
        public string? Email { get; set; }
        [ForeignKey("ContactPersonId")]
        public ContactPerson? person { get; set;}
    }
}
