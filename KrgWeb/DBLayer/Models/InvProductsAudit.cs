using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("InvProducts_Audit")]
public partial class InvProductsAudit
{
    [Key]
    [Column("AuditID")]
    public int AuditId { get; set; }

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

    [StringLength(1)]
    [Unicode(false)]
    public string? OperationType { get; set; }

    [ForeignKey("ProductCode")]
    [InverseProperty("InvProductsAudits")]
    public virtual InvProduct ProductCodeNavigation { get; set; } = null!;
}
