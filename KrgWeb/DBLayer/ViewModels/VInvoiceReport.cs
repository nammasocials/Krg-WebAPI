using System;
using System.Collections.Generic;

namespace DBLayer.ViewModels
{
    /// <summary>
    /// Flattened invoice data handed to the RDLC report. The header values become
    /// report parameters and <see cref="Items"/> becomes the "InvoiceItems"
    /// dataset, so property names here must match the field names in
    /// Reports\Invoice.rdlc.
    /// </summary>
    public class VInvoiceReport
    {
        public string InvoiceNo { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string CustomerGst { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string CompanyAddress { get; set; } = string.Empty;

        public string CompanyGst { get; set; } = string.Empty;

        /// <summary>Invoice-level total, as stored on [dbo].[InvInvoice].</summary>
        public decimal TotalCost { get; set; }

        public List<VInvoiceReportItem> Items { get; set; } = new List<VInvoiceReportItem>();
    }

    public class VInvoiceReportItem
    {
        public int SlNo { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string HsnCode { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitCost { get; set; }

        /// <summary>Quantity x UnitCost, before tax.</summary>
        public decimal Cost { get; set; }

        public decimal CentralGst { get; set; }

        public decimal CentralGstAmount { get; set; }

        public decimal StateGst { get; set; }

        public decimal StateGstAmount { get; set; }

        /// <summary>Cost + CGST + SGST.</summary>
        public decimal NetProductAmount { get; set; }
    }
}
