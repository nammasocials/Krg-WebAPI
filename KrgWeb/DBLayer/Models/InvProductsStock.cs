using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("InvProducts_Stock")]
public partial class InvProductsStock
{
    [Key]
    public Guid StockTnxId { get; set; }

    public Guid ProductCode { get; set; }

    public int Quantity { get; set; }

    public int TxnType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    [ForeignKey("ProductCode")]
    [InverseProperty("InvProductsStocks")]
    public virtual InvProduct ProductCodeNavigation { get; set; } = null!;
}
