using DomainLayer.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ProductsWithFiltersForCountSpec : BaseSpecification<Product, int>
    {
        public ProductsWithFiltersForCountSpec(ProductQueryParams Params)
            : base(p =>
                (!Params.BrandId.HasValue || p.BrandId == Params.BrandId) &&
                (!Params.TypeId.HasValue || p.TypeId == Params.TypeId) &&
                (string.IsNullOrEmpty(Params.Search) ||
                 p.Name.Contains(Params.Search) ||
                 p.Description.Contains(Params.Search))
            )
        {
        }
    }
}
