using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("InvCustomers_Audit")]
public partial class InvCustomersAudit
{
    [Key]
    [Column("AuditID")]
    public int AuditId { get; set; }

    public Guid CustomerCode { get; set; }

    [StringLength(100)]
    public string CustomerName { get; set; } = null!;

    [StringLength(100)]
    public string CustomerEmail { get; set; } = null!;

    [StringLength(15)]
    public string ContactNo { get; set; } = null!;

    [StringLength(15)]
    public string? SecnContactNo { get; set; }

    [StringLength(500)]
    public string CustomerAddress { get; set; } = null!;

    [Column("GST")]
    [StringLength(30)]
    public string Gst { get; set; } = null!;

    public byte[]? CustomerLogo { get; set; }

    [StringLength(30)]
    public string? CustomerLogoMime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string? OperationType { get; set; }

    [ForeignKey("CustomerCode")]
    [InverseProperty("InvCustomersAudits")]
    public virtual InvCustomer CustomerCodeNavigation { get; set; } = null!;
}
