using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class InvProductsStock
{
    public Guid StockTnxId { get; set; }

    public Guid ProductCode { get; set; }

    public int UnitType { get; set; }

    public int Quantity { get; set; }

    public int TxnType { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual InvProduct ProductCodeNavigation { get; set; } = null!;
}
