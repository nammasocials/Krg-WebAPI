using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

[Table("InvExpenses")]
public partial class InvExpense
{
    [Key]
    public Guid ExpenseCode { get; set; }

    /// <summary>[dbo].[InvConstant] key for EntityId 'InvExpenses', Category 'ExpenseType'.</summary>
    public int ExpenseType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ExpenseDate { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    /// <summary>Set when the expense belongs to one of our own invoices.</summary>
    public Guid? InvoiceCode { get; set; }

    [StringLength(200)]
    public string? VendorName { get; set; }

    [StringLength(100)]
    public string? VendorInvoiceNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VendorInvoiceDate { get; set; }

    [Column("isActive")]
    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
