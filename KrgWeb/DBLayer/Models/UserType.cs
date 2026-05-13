using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("UserType")]
public partial class UserType
{
    [Key]
    public short UserTypeId { get; set; }

    [StringLength(255)]
    public string UserTypeName { get; set; } = null!;

    [StringLength(500)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    [InverseProperty("UserTypeNavigation")]
    public virtual ICollection<InvUser> InvUsers { get; set; } = new List<InvUser>();
}
