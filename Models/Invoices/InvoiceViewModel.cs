using Smart_Invoice.Models.Products;

namespace Smart_Invoice.Models.Invoices
{
    [Serializable]
    public class InvoiceViewModel
    {
        public UtilityInvoice? UtilityInvoice { get; set; }
        public Product_Invoice? ProductInvoice { get; set; }

        public List<ProductMatches>? ProductMatches { get; set; }

    }
}
