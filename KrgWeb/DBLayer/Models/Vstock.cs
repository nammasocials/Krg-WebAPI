using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Keyless]
public partial class Vstock
{
    public Guid ProductCode { get; set; }

    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    [StringLength(200)]
    public string TransactionType { get; set; } = null!;

    public int Quantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }
}
