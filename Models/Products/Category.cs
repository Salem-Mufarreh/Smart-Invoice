using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Smart_Invoice.Models.Products
{
    public class Category
    {
        [Key]
        [Required]
        public long CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; } = string.Empty;
    }
}
