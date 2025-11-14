using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Service.Specifications;
using ServiceAbstraction;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Get All Products
        //public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        //{
        //    var repo = _unitOfWork.GetRepo<Product, int>(new Product());
        //    var products = await repo.GetAllAsync();
        //    return _mapper.Map<IEnumerable<ProductDto>>(products);
        //}

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var spec = new ProductsWithFiltersSpec();
            var repo = _unitOfWork.GetRepo<Product, int>(new Product());
            var products = await repo.GetAllAsync(spec);

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        //Get All Brands
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo = _unitOfWork.GetRepo<ProductBrand, int>(new ProductBrand());
            var brands = await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        //Get All Types
        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var repo = _unitOfWork.GetRepo<ProductType, int>(new ProductType());
            var types = await repo.GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDto>>(types);
        }

        //Get Product by ID
        //public async Task<ProductDetailsDto?> GetByIdAsync(int id)
        //{
        //    var repo = _unitOfWork.GetRepo<Product, int>(new Product());
        //    var product = await repo.GetByIdAsync(id);
        //    return product is null ? null : _mapper.Map<ProductDetailsDto>(product);
        //}

        public async Task<ProductDetailsDto?> GetByIdAsync(int id)
        {
            var spec = new ProductByIdSpec(id);
            var repo = _unitOfWork.GetRepo<Product, int>(new Product());

            var product = await repo.GetByIdAsync(spec);

            return product is null ? null : _mapper.Map<ProductDetailsDto>(product);
        }

        //Add Product
        public async Task AddAsync(CreateProductDto productDto)
        {
            var repo = _unitOfWork.GetRepo<Product, int>(new Product());
            var entity = _mapper.Map<Product>(productDto);
            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        //Update Product
        public async Task UpdateAsync(UpdateProductDto productDto)
        {
            var repo = _unitOfWork.GetRepo<Product, int>(new Product());
            var existing = await repo.GetByIdAsync(productDto.Id);

            if (existing is null)
                throw new KeyNotFoundException($"Product with ID {productDto.Id} not found.");

            _mapper.Map(productDto, existing);
            repo.Update(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        //Delete Product
        public async Task DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepo<Product, int>(new Product());
            var existing = await repo.GetByIdAsync(id);

            if (existing is null) return;

            repo.Remove(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
