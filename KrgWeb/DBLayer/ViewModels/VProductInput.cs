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
        public Guid ProductCode { get; set; }

        public string ProductName { get; set; } = null!;

        public int CurrentStock { get; set; }

        public int UnitType { get; set; }

        public decimal UnitCost { get; set; }
        public string Hsncode { get; set; } = null!;
        public IFormFile? ProductLogo { get; set; }
    }
    public class VProductAddStock
    {
        public Guid ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}
