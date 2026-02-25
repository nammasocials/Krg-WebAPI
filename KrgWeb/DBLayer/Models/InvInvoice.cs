using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class InvInvoice
{
    public Guid InvoiceCode { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public Guid CustomerCode { get; set; }

    public bool IsEwayBillAvailable { get; set; }

    public byte[]? EwayBillLogo { get; set; }

    public string? EwayBillLogoMime { get; set; }

    public decimal Gst { get; set; }

    public decimal TotalCost { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual InvCustomer CustomerCodeNavigation { get; set; } = null!;

    public virtual ICollection<InvInvoiceAudit> InvInvoiceAudits { get; set; } = new List<InvInvoiceAudit>();

    public virtual ICollection<InvInvoiceItem> InvInvoiceItems { get; set; } = new List<InvInvoiceItem>();
}
