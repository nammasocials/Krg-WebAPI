using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class VinvoiceDetail
{
    public Guid InvoiceCode { get; set; }

    public Guid ItemCode { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public Guid ProductCode { get; set; }

    public string ProductName { get; set; } = null!;

    public byte[]? ProductLogo { get; set; }

    public string? ProductLogoMime { get; set; }

    public decimal UnitCost { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    public decimal CentralGst { get; set; }

    public decimal CentralGstAmount { get; set; }

    public decimal StateGst { get; set; }

    public decimal StateGstAmount { get; set; }

    public string Hsncode { get; set; } = null!;

    public decimal TotalCost { get; set; }

    public string Gst { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
