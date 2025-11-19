using AutoMapper;
using DomainLayer.Models.BasketModels;
using Shared.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {

            CreateMap<CustomerBasket, BasketDto>();
            CreateMap<BasketItem, BasketItemDto>();

            CreateMap<BasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
        }
    }
}