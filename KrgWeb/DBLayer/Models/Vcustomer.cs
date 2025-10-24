using System;
using System.Collections.Generic;

namespace DBLayer.Models;

public partial class Vcustomer
{
    public int CustomerCode { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerAddress { get; set; } = null!;

    public string Gst { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public int? CreatedBy { get; set; }
}
