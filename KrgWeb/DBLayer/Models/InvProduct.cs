using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

public partial class InvProduct
{
    [Key]
    public Guid ProductCode { get; set; }

    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal UnitCost { get; set; }

    [Column("HSNCode")]
    [StringLength(10)]
    public string Hsncode { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal CentralGstPer { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal StateGstPer { get; set; }

    public int CurrentStock { get; set; }

    public byte[]? ProductLogo { get; set; }

    [StringLength(30)]
    public string? ProductLogoMime { get; set; }

    [Column("isActive")]
    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    [InverseProperty("ProductCodeNavigation")]
    public virtual ICollection<InvInvoiceItem> InvInvoiceItems { get; set; } = new List<InvInvoiceItem>();

    [InverseProperty("ProductCodeNavigation")]
    public virtual ICollection<InvProductsAudit> InvProductsAudits { get; set; } = new List<InvProductsAudit>();

    [InverseProperty("ProductCodeNavigation")]
    public virtual ICollection<InvProductsStock> InvProductsStocks { get; set; } = new List<InvProductsStock>();
}
