using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.ViewModels
{
    public class VCustomerInput
    {

        public string CustomerName { get; set; } = null!;

        public string CustomerAddress { get; set; } = null!;

        public string CustomerEmail { get; set; } = null!;

        public string ContactNo { get; set; } = null!;

        public string? SecnContactNo { get; set; }

        public string Gst { get; set; } = null!;
        public IFormFile? CompanyLogo { get; set; }
    }
}
