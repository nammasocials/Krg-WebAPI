using DBLayer.Models;
using DBLayer.Service.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Service
{
    public interface IRedisStockService
    {

    }
    public class RedisStockService : IRedisStockService
    {
        private readonly StackExchange.Redis.IDatabase _redisDb;
        private readonly NsinvoiceBillingContext _context;
        private readonly IUserClaimsService _userClaimsService;

        public RedisStockService(IConnectionMultiplexer redis, NsinvoiceBillingContext context)
        {
            _redisDb = redis.GetDatabase();
            _context = context;
        }
        public async Task<int> GetStockAsync(Guid productId)
        {
            var key = $"stock:{productId}";

            // Redis operations use _redisDb
            var cachedStock = await _redisDb.StringGetAsync(key);
            if (cachedStock.HasValue)
            {
                return int.Parse(cachedStock);
            }

            // EF operations use _dbContext
            var stock = await _context.InvProducts
                .Where(p => p.ProductCode == productId)
                .Select(p => p.CurrentStock)
                .FirstOrDefaultAsync();

            await _redisDb.StringSetAsync(key, stock.ToString(), TimeSpan.FromMinutes(30));
            return stock;
        }
    }
}
