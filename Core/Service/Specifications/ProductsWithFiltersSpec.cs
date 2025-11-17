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
        public ProductsWithFiltersSpec(int? brandId, int? typeId, ProductSortingOptions SortingOoption)
            : base(p =>
                (!brandId.HasValue || p.BrandId == brandId) &&
                (!typeId.HasValue || p.TypeId == typeId)
            )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            switch (SortingOoption)
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
