using System.ComponentModel.DataAnnotations;

namespace Smart_Invoice.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string? CustomerName { get; set; }
        public string? Customer_Name_Normilized { get; set; }
        public string? Customer_Name_English { get; set; }
        [Required]
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Store_Tax_Number { get; set;}
        public string? customer_number { get; set; }



    }
}
