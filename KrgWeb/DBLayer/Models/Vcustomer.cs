using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Keyless]
public partial class Vcustomer
{
    public Guid CustomerCode { get; set; }

    [StringLength(100)]
    public string CustomerName { get; set; } = null!;

    [StringLength(500)]
    public string CustomerAddress { get; set; } = null!;

    [StringLength(100)]
    public string CustomerEmail { get; set; } = null!;

    [StringLength(15)]
    public string ContactNo { get; set; } = null!;

    [StringLength(15)]
    public string? SecnContactNo { get; set; }

    [Column("GST")]
    [StringLength(30)]
    public string Gst { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
