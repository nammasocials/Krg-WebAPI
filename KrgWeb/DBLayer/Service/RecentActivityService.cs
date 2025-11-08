using DBLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Service
{
    public interface IRecentActivityService
    {
    }
    public class RecentActivityService : IRecentActivityService
    {
        private readonly NsinvoiceBillingContext _context;
        public RecentActivityService(NsinvoiceBillingContext context) 
        { 

        }
    }
}
