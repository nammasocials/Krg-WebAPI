using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class ActivityLog
{
    public int ActivityId { get; set; }

    public string EntityType { get; set; } = null!;

    public int EntityId { get; set; }

    public string ActionType { get; set; } = null!;

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? RedirectUrl { get; set; }
}
