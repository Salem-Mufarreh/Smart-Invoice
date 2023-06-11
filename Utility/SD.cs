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

        public const string ProductPrompt = "Can you restructure the given model as an invoice and  return the values as a key-value JSON object," +
            " where the keys are predefined? The model contains the following fields:" +
            " [Company (as object)contains ''Company_Name','Company_Name_English','Address','Phone','Fax','Company_License_Registration_Number']," +
            "'invoice_number', 'Invoice_Date', 'Subtotal', 'Tax', 'Total', " +
            "Items[(as Object) contains 'Name', 'Quantity','Unit','UnitPrice','Total'] and the invoice Currency(if not available default is ILS) ." +
            " Please provide the JSON structure for the invoice with the given fields and their corresponding values." +
            "the data will be in the follow up prompt ";


        public const string Productprompt_v2 = "Input: Generate the JSON structure for the invoice with the given fields and values.\\r\\n\\r\\nFields:\\r\\n- Company (Object):\\r\\n    - Company_Name (string)\\r\\n    - Company_Name_English (string)\\r\\n    - Address (string)\\r\\n    - Phone (string)\\r\\n    - Fax (string)\\r\\n    - Company_License_Registration_Number (string)\\r\\n- invoice_number (string)\\r\\n- Invoice_Date (string)\\r\\n- Subtotal (double)\\r\\n- Tax (double)\\r\\n- Total (double)\\r\\n- Items (Array of Objects):\\r\\n    - Name (string)\\r\\n    - Quantity (Integer)\\r\\n    - Unit (string)\\r\\n    - UnitPrice (double)\\r\\n    - Total (double)\\r\\n- Currency (string) default is(ILS)\\r\\n\\r\\nPlease generate the JSON structure for the invoice as per the given fields and values.\\r\\n\\r\\nInstructions:\\r\\n1. Extract the product names from the document. Product names should start with a capital letter.\\r\\n2. Ensure that product names are not phrases or sentences.\\r\\n3. Use the extracted product names to populate the 'Name' field in the 'Items' array.\\r\\n4. Generate the JSON structure with the provided fields and values.\";";
        public const string CheckForItemsPrompt = "you have two lists please return the best match for the invoice product form the database:";
        public const string CheckForItemsPromptC = "return the response as json format structured like the following (ProductMatches : [product(),bestmatch()]) if it was null keep it null";
        /* Toast Type */
        public const string ToastError = "error";
        public const string ToastSuccess = "success";

        /* WareHouse */
        public const string WarehouseActive = "Active";
        public const string WarehouseInActive = "InActive";
        public const string WarehouseMaintenance = "Maintenance";

    }
}
