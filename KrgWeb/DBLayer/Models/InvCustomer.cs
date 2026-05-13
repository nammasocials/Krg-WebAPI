using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

public partial class InvCustomer
{
    [Key]
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

    [InverseProperty("CustomerCodeNavigation")]
    public virtual ICollection<InvCustomersAudit> InvCustomersAudits { get; set; } = new List<InvCustomersAudit>();

    [InverseProperty("CustomerCodeNavigation")]
    public virtual ICollection<InvInvoice> InvInvoices { get; set; } = new List<InvInvoice>();
}
