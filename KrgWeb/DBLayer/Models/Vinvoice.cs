using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class Vinvoice
{
    public Guid InvoiceCode { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public bool IsEwayBillAvailable { get; set; }

    public decimal TotalCost { get; set; }

    public decimal Gst { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
