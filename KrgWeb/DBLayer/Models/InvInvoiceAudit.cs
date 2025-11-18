using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class InvInvoiceAudit
{
    public int AuditId { get; set; }

    public Guid InvoiceCode { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public Guid CustomerCode { get; set; }

    public decimal TotalCost { get; set; }

    public string Gst { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public string? OperationType { get; set; }

    public virtual InvInvoice InvoiceCodeNavigation { get; set; } = null!;
}
