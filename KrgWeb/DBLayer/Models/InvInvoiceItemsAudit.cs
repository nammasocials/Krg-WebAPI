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

    public decimal Cost { get; set; }

    public string Hsncode { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public string? OperationType { get; set; }

    public virtual InvInvoiceItem ItemCodeNavigation { get; set; } = null!;
}
