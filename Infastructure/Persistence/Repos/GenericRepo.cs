using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repos
{
    internal class GenericRepo<TEntity, TKey>(StoreDbContext dbContext) : IGenericRepo<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task AddAsync(TEntity entity) => await dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id) => await dbContext.FindAsync<TEntity>(id);

        //////////////////
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> spec)
                => await SpecificationEvaluator<TEntity, TKey>.GetQuery(dbContext.Set<TEntity>(), spec).ToListAsync();

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> spec)
                => await SpecificationEvaluator<TEntity, TKey>.GetQuery(dbContext.Set<TEntity>(), spec).FirstOrDefaultAsync();
        /////////////////////

        public void Remove(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => dbContext?.Set<TEntity>().Update(entity);

        /////////////////////
        public async Task<int> CountAsync() => await dbContext.Set<TEntity>().CountAsync();

        public async Task<int> CountAsync(ISpecification<TEntity, TKey> spec)
            => await SpecificationEvaluator<TEntity, TKey>
                        .GetQuery(dbContext.Set<TEntity>(), spec)
                        .CountAsync();
        /////////////////////
    }
}
