using System.ComponentModel.DataAnnotations;

namespace Smart_Invoice.Models.Registered_Companies
{
    public class RegisteredCompany
    {
        [Key]
        public long CompanyCode { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string CompanyPhone { get; set;}
        [Required]
        public string CompanyAddress { get; set; }
        public long GovCompanyRegistration { get; set; }

    }
}
