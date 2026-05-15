using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("InvInvoice_Audit")]
public partial class InvInvoiceAudit
{
    [Key]
    [Column("AuditID")]
    public int AuditId { get; set; }

    public Guid InvoiceCode { get; set; }

    [StringLength(100)]
    public string InvoiceNo { get; set; } = null!;

    public Guid CustomerCode { get; set; }

    [Column("isEwayBillAvailable")]
    public bool IsEwayBillAvailable { get; set; }

    [Column("EWayBillLogo")]
    public byte[]? EwayBillLogo { get; set; }

    [Column("EWayBillLogoMime")]
    [StringLength(30)]
    public string? EwayBillLogoMime { get; set; }

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

    [StringLength(1)]
    [Unicode(false)]
    public string? OperationType { get; set; }

    [ForeignKey("InvoiceCode")]
    [InverseProperty("InvInvoiceAudits")]
    public virtual InvInvoice InvoiceCodeNavigation { get; set; } = null!;
}
