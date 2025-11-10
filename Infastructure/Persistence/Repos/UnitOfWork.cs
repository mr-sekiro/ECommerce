using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repos
{
    public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> repos = new();
        public IGenericRepo<TEntity, TKey> GetRepo<TEntity, TKey>(TEntity entity) where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;

            if (repos.TryGetValue(typeName, out var existingRepo))
                return (IGenericRepo<TEntity, TKey>)existingRepo;

            var newRepo = new GenericRepo<TEntity, TKey>(dbContext);
            repos[typeName] = newRepo;

            return newRepo;
        }

        public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();

    }
}
