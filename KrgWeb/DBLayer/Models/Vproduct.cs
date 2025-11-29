using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class Vproduct
{
    public Guid ProductCode { get; set; }

    public string ProductName { get; set; } = null!;

    public int CurrentStock { get; set; }

    public string Hsncode { get; set; } = null!;

    public decimal UnitCost { get; set; }

    public decimal CentralGstPer { get; set; }

    public decimal StateGstPer { get; set; }

    public decimal? IntraStateTotal { get; set; }

    public decimal? InterStateTotal { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
