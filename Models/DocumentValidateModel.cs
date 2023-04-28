namespace Smart_Invoice.Models
{
    public class DocumentValidateModel
    {
        public Google.Cloud.DocumentAI.V1.Document Document { get; set; }
        public IFormFile File { get; set; }
        public string? Image { get; set; }

    }
}
