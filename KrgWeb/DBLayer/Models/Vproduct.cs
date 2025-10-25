using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class Vproduct
{
    public int ProductCode { get; set; }

    public string ProductName { get; set; } = null!;

    public int StockCount { get; set; }

    public string UnitName { get; set; } = null!;

    public decimal UnitCost { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
