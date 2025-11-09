using DBLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Service
{
    public interface IRecentActivityService
    {
        public Task<VactivityLog> AddActivityLogAsync(ActivityLog activityLog);
        public Task<List<VactivityLog>> fetchActivityLogsList();
        public Task<List<VactivityLog>> fetchActivityLogsByEntity(string entity);
        public Task<VactivityLog> fetchActivityLogsByLogId(int logId);
        public Task<string> GetChangedColumns(EntityEntry entry, params string[] ignoreColumns);
    }
    public class RecentActivityService : IRecentActivityService
    {
        private readonly NsinvoiceBillingContext _context;
        public RecentActivityService(NsinvoiceBillingContext context) 
        {
            _context = context;
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
        public async Task<string> GetChangedColumns(EntityEntry entry, params string[] ignoreColumns)
        {
            var changedColumns = entry.Properties
            .Where(p =>
                p.IsModified &&
                !Equals(p.CurrentValue, p.OriginalValue) &&
                (ignoreColumns == null || !ignoreColumns.Contains(p.Metadata.Name))
            )
            .Select(p => p.Metadata.Name)
            .ToList();

            // Format: "Name, Email and GSTNo"
            return changedColumns.Count switch
            {
                0 => string.Empty,
                1 => changedColumns[0],
                _ => string.Join(", ", changedColumns.Take(changedColumns.Count - 1))
                        + " and " + changedColumns.Last()
            };
        }
        public async Task<VactivityLog> AddActivityLogAsync(ActivityLog activityLog)
        {
            await _context.ActivityLogs.AddAsync(activityLog);
            await _context.SaveChangesAsync();
            return await fetchActivityLogsByLogId(activityLog.ActivityId);
        }
    }
}
