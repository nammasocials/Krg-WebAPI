using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Keyless]
public partial class Vconstant
{
    public int ConstantId { get; set; }

    [StringLength(100)]
    public string EntityId { get; set; } = null!;

    [StringLength(100)]
    public string Category { get; set; } = null!;

    public int Key { get; set; }

    [StringLength(200)]
    public string? ConstantValue { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string? PluralName { get; set; }

    [StringLength(200)]
    public string ShName { get; set; } = null!;

    [StringLength(200)]
    public string? ShPluralName { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [Column("isActive")]
    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }
}
