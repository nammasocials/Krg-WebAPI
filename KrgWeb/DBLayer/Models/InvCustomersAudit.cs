using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class InvCustomersAudit
{
    public int AuditId { get; set; }

    public int? CustomerCode { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerEmail { get; set; } = null!;

    public string ContactNo { get; set; } = null!;

    public string? SecnContactNo { get; set; }

    public string CustomerAddress { get; set; } = null!;

    public string Gst { get; set; } = null!;

    public byte[]? CustomerLogo { get; set; }

    public string? CustomerLogoMime { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public string? OperationType { get; set; }
}
