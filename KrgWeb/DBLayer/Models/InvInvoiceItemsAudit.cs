using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class InvInvoiceItemsAudit
{
    public int AuditId { get; set; }

    public Guid ItemCode { get; set; }

    public Guid InvoiceCode { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public Guid ProductCode { get; set; }

    public decimal UnitCost { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    public string Hsncode { get; set; } = null!;

    public decimal CentralGst { get; set; }

    public decimal CentralGstAmount { get; set; }

    public decimal StateGst { get; set; }

    public decimal StateGstAmount { get; set; }

    public decimal NetProductAmount { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public string? OperationType { get; set; }

    public virtual InvInvoiceItem ItemCodeNavigation { get; set; } = null!;
}
