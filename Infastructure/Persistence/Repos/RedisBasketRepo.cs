using DomainLayer.Contracts;
using DomainLayer.Models.BasketModels;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Persistence.Repos
{
    public class RedisBasketRepo : IRedisBasketRepo
    {
        private readonly IDatabase _db;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public RedisBasketRepo(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<CustomerBasket?> GetBasketAsync(string Key)
        {
            var data = await _db.StringGetAsync(Key);
            if (data.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<CustomerBasket>(data!, _jsonOptions);
        }

        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? TimeToLive)
        {
            var created = await _db.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket, _jsonOptions), TimeToLive ?? TimeSpan.FromDays(30));
            if (!created) return null;
            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _db.KeyDeleteAsync(id);
        }
    }
}
