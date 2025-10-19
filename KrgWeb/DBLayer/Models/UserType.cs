using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class UserType
{
    public short UserTypeId { get; set; }

    public string UserTypeName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public virtual ICollection<InvUser> InvUsers { get; set; } = new List<InvUser>();
}
