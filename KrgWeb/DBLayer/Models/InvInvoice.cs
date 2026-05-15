using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("InvInvoice")]
[Index("InvoiceNo", Name = "UQ__InvInvoi__D796B22779F996C4", IsUnique = true)]
public partial class InvInvoice
{
    [Key]
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

    [Column("GST")]
    [StringLength(30)]
    public string Gst { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalCost { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    [ForeignKey("CustomerCode")]
    [InverseProperty("InvInvoices")]
    public virtual InvCustomer CustomerCodeNavigation { get; set; } = null!;

    [InverseProperty("InvoiceCodeNavigation")]
    public virtual ICollection<InvInvoiceAudit> InvInvoiceAudits { get; set; } = new List<InvInvoiceAudit>();

    [InverseProperty("InvoiceCodeNavigation")]
    public virtual ICollection<InvInvoiceItem> InvInvoiceItems { get; set; } = new List<InvInvoiceItem>();
}
