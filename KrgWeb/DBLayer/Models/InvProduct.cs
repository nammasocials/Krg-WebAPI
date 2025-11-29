using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class InvProduct
{
    public Guid ProductCode { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal UnitCost { get; set; }

    public string Hsncode { get; set; } = null!;

    public decimal CentralGstPer { get; set; }

    public decimal StateGstPer { get; set; }

    public int CurrentStock { get; set; }

    public byte[]? ProductLogo { get; set; }

    public string? ProductLogoMime { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual ICollection<InvInvoiceItem> InvInvoiceItems { get; set; } = new List<InvInvoiceItem>();

    public virtual ICollection<InvProductsAudit> InvProductsAudits { get; set; } = new List<InvProductsAudit>();

    public virtual ICollection<InvProductsStock> InvProductsStocks { get; set; } = new List<InvProductsStock>();
}
