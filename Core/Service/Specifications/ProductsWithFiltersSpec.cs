using DomainLayer.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ProductsWithFiltersSpec : BaseSpecification<Product, int>
    {
        public ProductsWithFiltersSpec(ProductQueryParams Params)
            : base(p =>
                (!Params.BrandId.HasValue || p.BrandId == Params.BrandId) &&
                (!Params.TypeId.HasValue || p.TypeId == Params.TypeId) &&
                (string.IsNullOrEmpty(Params.Search) ||
                 p.Name.Contains(Params.Search) ||
                 p.Description.Contains(Params.Search))
            )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            switch (Params.SortingOption)
            {
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;

                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;

                case ProductSortingOptions.Name:
                    AddOrderBy(p => p.Name);
                    break;

                default:
                    AddOrderBy(p => p.Name); // default behavior
                    break;
            }

        }
    }
}
