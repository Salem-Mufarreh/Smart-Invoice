namespace Smart_Invoice.Utility
{
    /* Enter here all the hard coded strings as a variables */
    public class SD
    {
        /* Roles */
        public const string Role_Admin = "Admin";
        public const string Role_Accountant = "Accountant";



        /* Status */
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        /* Payments */
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";
        public const string PaymentStatusUnPaid = "UnPaid";
        public const string PaymentStatusPartiallyPaid = "Partially Paid";
        public const string PaymentStatusPaid = "Paid";

        /* Currancy */
        public const string CurrancyCodeILS = "001";
        public const string CurrancyCodeUSD = "002";
        public const string CurrancyNameUSD = "USD";
        public const string CurrancyNameILS = "ILS";

        /* Invoice Types */
        public const string InvoiceUtility = "Utility";
        public const string InvoiceProduct = "Product";
        public const string InvoiceService = "Service";

        /* OpenAI Prompt */
        public const string UtilityPrompt = "Can you restructure the given model as an invoice and" +
            " return the values as a key-value JSON object, where the keys are predefined? The model" +
            " contains the following fields: [Company (as object),invoice_number, service-number," +
            "invoice_date,Meter_Number,Previous_Reading_Date,Current_Reading,Category,VAT,Previous_Debt,Paid_Amount]. " +
            "Please provide the JSON structure for the invoice with the given fields and their corresponding values.";
        public const string ProductPrompt = "Can you restructure the given model as an invoice and" +
            " return the values as a key-value JSON object, where the keys are predefined? The model" +
            " contains the following fields: [Company (as object),invoice_number, Invoice_Date, Subtotal, Tax, Total, Items(as Object)]. " +
            "Please provide the JSON structure for the invoice with the given fields and their corresponding values.";

        /* Toast Type */
        public const string ToastError = "error";
        public const string ToastSuccess = "success";

        /* WareHouse */
        public const string WarehouseActive = "Active";
        public const string WarehouseInActive = "InActive";
        public const string WarehouseMaintenance = "Maintenance";

    }
}
