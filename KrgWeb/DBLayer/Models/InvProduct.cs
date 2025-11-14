using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class InvProduct
{
    public Guid ProductCode { get; set; }

    public string ProductName { get; set; } = null!;

    public int UnitType { get; set; }

    public decimal UnitCost { get; set; }

    public int CurrentStock { get; set; }

    public byte[]? ProductLogo { get; set; }

    public string? ProductLogoMime { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual ICollection<InvProductsAudit> InvProductsAudits { get; set; } = new List<InvProductsAudit>();

    public virtual ICollection<InvProductsStock> InvProductsStocks { get; set; } = new List<InvProductsStock>();
}
