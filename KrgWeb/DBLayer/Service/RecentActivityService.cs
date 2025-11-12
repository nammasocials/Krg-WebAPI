using DBLayer.Models;
using DBLayer.Service.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Service
{
    public interface IRecentActivityService
    {
        public Task<VactivityLog> AddActivityLogAsync(string entityType, Guid entityCode,
            string actionType, string description);
        public Task<List<VactivityLog>> fetchActivityLogsList();
        public Task<List<VactivityLog>> fetchActivityLogsByEntity(string entity);
        public Task<VactivityLog> fetchActivityLogsByLogId(int logId);
    }
    public class RecentActivityService : IRecentActivityService
    {
        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;
        public RecentActivityService(NsinvoiceBillingContext context, IUserClaimsService iUserClaimsService) 
        {
            _context = context;
            _userClaimsService = iUserClaimsService;
        }
        public async Task<List<VactivityLog>> fetchActivityLogsList()
        {
            var activityLogs = await _context.VactivityLogs.ToListAsync();
            return activityLogs;
        }
        public async Task<List<VactivityLog>> fetchActivityLogsByEntity(string entity)
        {
            var activityLog = await _context.VactivityLogs.Where(C => C.EntityType == entity).ToListAsync();
            return activityLog;
        }
        public async Task<VactivityLog> fetchActivityLogsByLogId(int logId)
        {
            var activityLog = await _context.VactivityLogs.Where(C => C.ActivityId == logId).FirstOrDefaultAsync();
            return activityLog;
        }
        public async Task<VactivityLog> AddActivityLogAsync(string entityType, Guid entityCode, 
            string actionType, string description)
        {
            var claims = _userClaimsService.GetUserClaims();
            var activityLog = new ActivityLog()
            {
                EntityType = entityType,
                EntityId = entityCode,
                ActionType = actionType,
                Description = description,
                CreatedBy = claims.UserCode
            };
            await _context.ActivityLogs.AddAsync(activityLog);
            await _context.SaveChangesAsync();
            return await fetchActivityLogsByLogId(activityLog.ActivityId);
        }
    }
}
