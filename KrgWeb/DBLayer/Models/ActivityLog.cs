using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("ActivityLog")]
public partial class ActivityLog
{
    [Key]
    public int ActivityId { get; set; }

    [StringLength(50)]
    public string EntityType { get; set; } = null!;

    public Guid EntityId { get; set; }

    [StringLength(20)]
    public string ActionType { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [StringLength(300)]
    public string? RedirectUrl { get; set; }
}
