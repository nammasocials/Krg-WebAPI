using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DBLayer.ViewModels
{
    public class VProductInput
    {
        public int ProductCode { get; set; }

        public string ProductName { get; set; } = null!;

        public int StockCount { get; set; }

        public string UnitName { get; set; } = null!;

        public decimal UnitCost { get; set; }
        public IFormFile? ProductLogo { get; set; }
    }
}
