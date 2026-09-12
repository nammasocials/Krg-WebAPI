using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Keyless]
public partial class Vexpense
{
    public Guid ExpenseCode { get; set; }

    public int ExpenseType { get; set; }

    [StringLength(200)]
    public string ExpenseTypeName { get; set; } = null!;

    [StringLength(200)]
    public string ExpenseTypeShortName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ExpenseDate { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public Guid? InvoiceCode { get; set; }

    [StringLength(100)]
    public string? InvoiceNo { get; set; }

    [StringLength(100)]
    public string? CustomerName { get; set; }

    [StringLength(200)]
    public string? VendorName { get; set; }

    [StringLength(100)]
    public string? VendorInvoiceNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VendorInvoiceDate { get; set; }

    /// <summary>'Normal' or 'Invoice Based'.</summary>
    [StringLength(13)]
    public string ExpenseNature { get; set; } = null!;

    [Column("isActive")]
    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
