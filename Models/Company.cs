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
        public string? Address { get; set; }
        public string? Company_License_Registration_Number { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }
        [ForeignKey("ContactPersonId")]
        public ContactPerson person { get; set;}
    }
}
