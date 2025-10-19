using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class InvUser
{
    public Guid UserCode { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public short UserType { get; set; }

    public DateTime CreatedOn { get; set; }

    public virtual UserType UserTypeNavigation { get; set; } = null!;
}
