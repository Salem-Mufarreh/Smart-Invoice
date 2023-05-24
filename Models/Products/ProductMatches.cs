namespace Smart_Invoice.Models.Products
{
    public class ProductMatches
    {
        public string Product { get; set; }
        public string Bestmatch { get; set; }
    }

    public class ListProductMatches
    {
        public List<ProductMatches> ProductMatches { get; set;}
    }
}
