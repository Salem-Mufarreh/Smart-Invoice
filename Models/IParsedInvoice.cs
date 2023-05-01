namespace Smart_Invoice.Models
{
    public interface IParsedInvoice
    {
        string? Id { get; set; }
        string? Invoice_Number { get; set; }
        string? Invoice_Date { get; set; }
        string? Invoice_Client_Name { get; set; }
        string? Incoive_Company { get; set; }
        string? Company_Address { get; set; }
        string? Company_License_Registration_Number { get; set; }
        double? Invoice_Amount { get; set; }
        double? Invoice_VAT { get; set; }


    }
}
