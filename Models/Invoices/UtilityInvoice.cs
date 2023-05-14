// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Invoice.Models.Invoices
{
    [Serializable]
    [Table("UtilityInvoices")]
    public class UtilityInvoice : Invoice
    {
       
        public string? Service_Number { get; set; }
        public string? Meter_Number { get; set; }
        public string? Previous_Reading_Date { get; set; }
        public string? Current_Reading { get; set; }
        public string? Category { get; set; }
        public string? Previous_Debt { get; set; }


    }
}
