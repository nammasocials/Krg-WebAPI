using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class Vstock
{
    public Guid ProductCode { get; set; }

    public string ProductName { get; set; } = null!;

    public int UnitType { get; set; }

    public string UnitName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string? PluralUnitName { get; set; }

    public string? PluralShortName { get; set; }

    public string UnitNameDetail { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public string StockDisplay { get; set; } = null!;
}
