using DomainLayer.Contracts;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecification() { }

        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new();

        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
            => Includes.Add(includeExpression);

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
            => OrderBy = orderByExpression;

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
            => OrderByDescending = orderByDescExpression;

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
