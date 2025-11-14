using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ProductsWithFiltersSpec : BaseSpecification<Product, int>
    {
        public ProductsWithFiltersSpec()
        {
            AddInclude(p => p.ProductBrand);   
            AddInclude(p => p.ProductType);    

            AddOrderBy(p => p.Name);
        }
    }
}
