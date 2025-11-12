using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class Vcustomer
{
    public Guid CustomerCode { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerAddress { get; set; } = null!;

    public string CustomerEmail { get; set; } = null!;

    public string ContactNo { get; set; } = null!;

    public string? SecnContactNo { get; set; }

    public string Gst { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }
}
