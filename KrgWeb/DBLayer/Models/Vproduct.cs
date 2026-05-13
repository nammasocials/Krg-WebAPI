using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Keyless]
public partial class Vproduct
{
    public Guid ProductCode { get; set; }

    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    public int CurrentStock { get; set; }

    [Column("HSNCode")]
    [StringLength(10)]
    public string Hsncode { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal UnitCost { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal CentralGstPer { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal StateGstPer { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? IntraStateTotal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? InterStateTotal { get; set; }

    [Column("isActive")]
    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
