using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class InvProductsConstant
{
    public int ConstantId { get; set; }

    public string? Category { get; set; }

    public int Key { get; set; }

    public string Name { get; set; } = null!;

    public string ShName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }
}
