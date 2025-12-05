using DBLayer.Models;
using DBLayer.Profiler;
using DBLayer.ViewModels;
using LiteDB;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBLayer.Service
{
    public interface ICustomerCacheService
    {
        public Task<Vcustomer?> GetCurrentCustomerByCodeAsync(string customerCode);
        public Task<List<Vcustomer>> GetCurrentCustomersAsync();
        public Task AddCustomerToCacheAsync(Vcustomer customer);

    }
    public class CustomerCacheService : ICustomerCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private const string CUSTOMER_CACHE_KEY_PREFIX = "customer_";
        private static readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);
        private readonly ICustomerPersistentQueueService _queueService;
        public CustomerCacheService(IMemoryCache memoryCache, ICustomerPersistentQueueService queueService)
        {
            _memoryCache = memoryCache;
            _queueService = queueService;
        }
        public async Task<Vcustomer?> GetCurrentCustomerByCodeAsync(string customerCode)
        {
            var cacheKey = $"{CUSTOMER_CACHE_KEY_PREFIX}{customerCode}";

            if (_memoryCache.TryGetValue(cacheKey, out Vcustomer cachedCustomer))
            {
                return cachedCustomer;
            }

            return null; // Cache miss - you may want to load from DB here
        }
        public async Task<List<Vcustomer>> GetCurrentCustomersAsync()
        {
            var cacheKey = $"{CUSTOMER_CACHE_KEY_PREFIX}";

            if (_memoryCache.TryGetValue(cacheKey, out List<Vcustomer> cachedCustomer))
            {
                return cachedCustomer;
            }

            return null; // Cache miss - you may want to load from DB here
        }
        public async Task AddCustomerToCacheAsync(Vcustomer customer)
        {
            var customerToCache = CustomerMapper.ToDto(customer);
            await _cacheLock.WaitAsync();
            try
            {
                var cacheKey = $"{CUSTOMER_CACHE_KEY_PREFIX}{customerToCache.CustomerCode}";

                // Check if customer already exists in cache
                if (_memoryCache.TryGetValue(cacheKey, out VCustomerCache existingCustomer))
                {
                    if (existingCustomer.IsSyncedToDb)
                    {
                        return; // Already in DB, ignore
                    }
                    else
                    {
                        return; // Already pending, ignore duplicate
                    }
                }

                // Create pending operation for DB insert
                var operationId = Guid.NewGuid();
                var operation = new VMPendingOperation
                {
                    Id = operationId,
                    OperationType = "INSERT_CUSTOMER",
                    JsonData = System.Text.Json.JsonSerializer.Serialize(customerToCache),
                    CreatedAt = DateTime.UtcNow,
                    RetryCount = 0,
                };

                await _queueService.EnqueueAsync(operation);

                _memoryCache.Set(cacheKey, customerToCache, TimeSpan.FromHours(24));
            }
            finally
            {
                _cacheLock.Release();
            }
        }
    }
    public interface ICustomerPersistentQueueService
    {
        Task EnqueueAsync(VMPendingOperation operation);
        Task<List<VMPendingOperation>> DequeueAsync(int batchSize);
        Task RemoveAsync(Guid operationId);
        Task UpdateRetryCountAsync(Guid operationId);
    }
    public class CustomerPersistentQueueService : ICustomerPersistentQueueService
    {
        private readonly string _dbPath;
        private readonly SemaphoreSlim _dbLock = new SemaphoreSlim(1, 1);
        public CustomerPersistentQueueService(IConfiguration configuration)
        {
            _dbPath = configuration["QueueDatabase:Path"] ?? "queue.db";
        }
        public async Task EnqueueAsync(VMPendingOperation operation)
        {
            await _dbLock.WaitAsync();
            try
            {
                using var db = new LiteDatabase(_dbPath);
                var collection = db.GetCollection<VMPendingOperation>("pending_operations");
                collection.Insert(operation);
            }
            finally
            {
                _dbLock.Release();
            }
        }

        public async Task<List<VMPendingOperation>> DequeueAsync(int batchSize)
        {
            await _dbLock.WaitAsync();
            try
            {
                using var db = new LiteDatabase(_dbPath);
                var collection = db.GetCollection<VMPendingOperation>("pending_operations");
                return collection.Query()
                    .Where(x => x.RetryCount < 5)
                    .OrderBy(x => x.CreatedAt)
                    .Limit(batchSize)
                    .ToList();
            }
            finally
            {
                _dbLock.Release();
            }
        }

        public async Task RemoveAsync(Guid operationId)
        {
            await _dbLock.WaitAsync();
            try
            {
                using var db = new LiteDatabase(_dbPath);
                var collection = db.GetCollection<VMPendingOperation>("pending_operations");
                collection.Delete(operationId);
            }
            finally
            {
                _dbLock.Release();
            }
        }

        public async Task UpdateRetryCountAsync(Guid operationId)
        {
            await _dbLock.WaitAsync();
            try
            {
                using var db = new LiteDatabase(_dbPath);
                var collection = db.GetCollection<VMPendingOperation>("pending_operations");
                var operation = collection.FindById(operationId);
                if (operation != null)
                {
                    operation.RetryCount++;
                    collection.Update(operation);
                }
            }
            finally
            {
                _dbLock.Release();
            }
        }
    }
}
