using DBLayer.Models;
using DBLayer.Service.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Service
{
    public interface ICommonService
    {
        public Task<List<Vconstant>> getProductUnitTypeAsync();
    }
    public class CommonService : ICommonService
    {
        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;
        public CommonService(NsinvoiceBillingContext context, IUserClaimsService iUserClaimsService)
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
        }
        public async Task<List<Vconstant>> getProductUnitTypeAsync()
        {
            return await _context.Vconstants.Where(C => C.EntityId == "InvProducts" && C.Category == "UnitType").ToListAsync();
        }
    }
}
