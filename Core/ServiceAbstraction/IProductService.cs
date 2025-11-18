using Shared;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        //get all Products
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(ProductQueryParams Params);
        Task<PaginatedResult<ProductDto>> GetProductsWithPaginationAsync(ProductQueryParams Params);
        Task<int> GetProductsCountAsync(ProductQueryParams Params);
        //get all Brands
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        //get all Types
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();
        //get Product by id
        Task<ProductDetailsDto?> GetByIdAsync(int id);
        Task AddAsync(CreateProductDto productDto);
        Task UpdateAsync(UpdateProductDto productDto);
        Task DeleteAsync(int id);
    }
}
