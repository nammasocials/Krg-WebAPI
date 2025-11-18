using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class Vproduct
{
    public Guid ProductCode { get; set; }

    public string ProductName { get; set; } = null!;

    public int CurrentStock { get; set; }

    public int UnitType { get; set; }

    public string UnitName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string? PluralUnitName { get; set; }

    public string? PluralShortName { get; set; }

    public string UnitNameDetail { get; set; } = null!;

    public string Hsncode { get; set; } = null!;

    public decimal UnitCost { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public string StockDisplay { get; set; } = null!;
}
