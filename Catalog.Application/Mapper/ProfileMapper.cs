using AutoMapper;
using Catalog.Application.Commands.Products;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.EntityParams;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Mapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<ProductBrand, BrandResponse>().ReverseMap();
            CreateMap<ProductType, TypeResponse>().ReverseMap();
            /*-------------Product----------*/
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
            CreateMap<Pagination<Product>, Pagination<ProductResponse>>();

            CreateMap<Pagination<Product>, Pagination<ProductResponse>>()
    .ConstructUsing((src, ctx) => new Pagination<ProductResponse>(
        src.PageIndex,
        src.PageSize,
        src.PageCount,
        src.Data.Select(item => ctx.Mapper.Map<ProductResponse>(item)).ToList()
    ));

        }
    }
}
