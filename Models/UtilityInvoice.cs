// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models
{
    [Serializable]
    public class UtilityInvoice : IParsedInvoice
    {
        [Key]
        public string? Id { get; set; }
        public string? Invoice_Number { get; set; }
        public string? Invoice_Date { get; set; }
        public string? Invoice_Client_Name { get; set; }
        public string? Incoive_Company { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company_Id { get; set; }
        public string? Company_Address { get; set; }
        public string? Company_License_Registration_Number { get; set; }
        public double? Invoice_Amount { get; set; }
        public double? Invoice_VAT { get; set; }
        public string? Service_Number { get; set; }
        public string? Meter_Number { get; set; }
        public string? Previous_Reading_Date { get; set; }
        public string? Current_Reading { get; set; }
        public string? Category { get; set; }
        public string? Previous_Debt { get; set; }

        
    }
}
