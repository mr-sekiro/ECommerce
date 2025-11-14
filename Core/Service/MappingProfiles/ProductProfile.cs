using AutoMapper;
using DomainLayer.Models;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Product
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ProductType.Name))
                //.ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => $"https://llocalhost:7169/{src.PictureUrl}"));
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<PictureUrlResolver>());

            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.ProductType.Name));

            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            // Brand & Type
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();
        }
    }
}


