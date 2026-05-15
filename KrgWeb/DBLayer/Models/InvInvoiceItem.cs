using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("InvInvoice_Items")]
public partial class InvInvoiceItem
{
    [Key]
    public Guid ItemCode { get; set; }

    public Guid InvoiceCode { get; set; }

    [StringLength(100)]
    public string InvoiceNo { get; set; } = null!;

    public Guid ProductCode { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal UnitCost { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(20, 4)")]
    public decimal Cost { get; set; }

    [Column("HSNCode")]
    [StringLength(10)]
    public string Hsncode { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal CentralGst { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal CentralGstAmount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal StateGst { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal StateGstAmount { get; set; }

    [Column(TypeName = "decimal(20, 4)")]
    public decimal NetProductAmount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    [InverseProperty("ItemCodeNavigation")]
    public virtual ICollection<InvInvoiceItemsAudit> InvInvoiceItemsAudits { get; set; } = new List<InvInvoiceItemsAudit>();

    [ForeignKey("InvoiceCode")]
    [InverseProperty("InvInvoiceItems")]
    public virtual InvInvoice InvoiceCodeNavigation { get; set; } = null!;

    [ForeignKey("ProductCode")]
    [InverseProperty("InvInvoiceItems")]
    public virtual InvProduct ProductCodeNavigation { get; set; } = null!;
}
