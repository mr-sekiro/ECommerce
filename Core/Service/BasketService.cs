using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models.BasketModels;
using ServiceAbstraction;
using Shared.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BasketService : IBasketService
    {
        private readonly IRedisBasketRepo _basketRepo;
        private readonly IMapper _mapper;

        public BasketService(IRedisBasketRepo basketRepo, IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }

        public async Task<BasketDto?> GetBasketAsync(string id)
        {
            var basket = await _basketRepo.GetBasketAsync(id);

            return basket is null
                ? null
                : _mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasket>(basketDto);

            var updatedBasket = await _basketRepo.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<BasketDto>(updatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _basketRepo.DeleteBasketAsync(id);
        }
    }
}
