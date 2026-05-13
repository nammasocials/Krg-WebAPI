using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("InvUser")]
public partial class InvUser
{
    [Key]
    public Guid UserCode { get; set; }

    [StringLength(100)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    [StringLength(200)]
    public string FullName { get; set; } = null!;

    public short UserType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    [ForeignKey("UserType")]
    [InverseProperty("InvUsers")]
    public virtual UserType UserTypeNavigation { get; set; } = null!;
}
