using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Keyless]
public partial class Vinvoice
{
    public Guid InvoiceCode { get; set; }

    [StringLength(100)]
    public string InvoiceNo { get; set; } = null!;

    [StringLength(100)]
    public string CustomerName { get; set; } = null!;

    public Guid CustomerCode { get; set; }

    [Column("GSTNumber")]
    [StringLength(30)]
    public string Gstnumber { get; set; } = null!;

    [StringLength(100)]
    public string CustomerEmail { get; set; } = null!;

    [Column("isEwayBillAvailable")]
    public bool IsEwayBillAvailable { get; set; }

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
