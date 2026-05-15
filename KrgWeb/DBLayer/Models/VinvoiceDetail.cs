using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Keyless]
public partial class VinvoiceDetail
{
    public Guid InvoiceCode { get; set; }

    public Guid ItemCode { get; set; }

    [StringLength(100)]
    public string InvoiceNo { get; set; } = null!;

    [StringLength(100)]
    public string CustomerName { get; set; } = null!;

    public Guid CustomerCode { get; set; }

    public Guid ProductCode { get; set; }

    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    public byte[]? ProductLogo { get; set; }

    [StringLength(30)]
    public string? ProductLogoMime { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal UnitCost { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(20, 4)")]
    public decimal Cost { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal CentralGst { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal CentralGstAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal StateGst { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal StateGstAmount { get; set; }

    [Column("HSNCode")]
    [StringLength(10)]
    public string Hsncode { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalCost { get; set; }

    [Column("GST")]
    [StringLength(30)]
    public string Gst { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
