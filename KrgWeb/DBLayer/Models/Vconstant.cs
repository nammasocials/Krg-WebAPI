using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class Vconstant
{
    public int ConstantId { get; set; }

    public string EntityId { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int Key { get; set; }

    public string Name { get; set; } = null!;

    public string? PluralName { get; set; }

    public string ShName { get; set; } = null!;

    public string? ShPluralName { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }
}
