using DBLayer.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBLayer.ViewModels
{
    public class VMPendingOperation
    {
        [BsonId]
        public Guid Id { get; set; }
        public string OperationType { get; set; } // "INSERT_STOCK" or "UPDATE_PRODUCT"
        public string JsonData { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RetryCount { get; set; }
    }
    public class VCustomerCache : Vcustomer
    {
        public bool IsSyncedToDb { get; set; } = false;
    }
}
