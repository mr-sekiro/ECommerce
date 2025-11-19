using AutoMapper;
using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IRedisBasketRepo redisBasketRepo)
        {
            _productService = new Lazy<IProductService>(() =>
                new ProductService(unitOfWork, mapper));

            _basketService = new Lazy<IBasketService>(() =>
               new BasketService(redisBasketRepo, mapper));
        }

        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _basketService.Value;
    }
}
