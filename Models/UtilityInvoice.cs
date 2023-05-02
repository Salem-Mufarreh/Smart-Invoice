namespace Smart_Invoice.Models
{
    [Serializable]
    public class UtilityInvoice
    {
        public Guid Id { get; set; }
        public string Company { get; set; }
        public string CompanyAddress { get; set; }
        public string VATDealer { get; set; }
        public string POS_Number { get; set; }
        public string Time { get;set; }
        public string Customer_Name { get; set; }
        public string Service_Number { get; set;}
        public string Meter_Number { get; set; }
        public string Previous_Reading_Date { get; set; }
        public string Current_Reading { get; set; }
        public string Category { get; set; }
        public string VAT { get; set; }
        public string Previous_Debt { get; set; }
        public string Paid_Amount { get; set; }




    }
}
