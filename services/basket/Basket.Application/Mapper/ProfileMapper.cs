using AutoMapper;
using Basket.Application.CQRS.Commands;
using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mapper
{
    public class ProfileMapper: Profile
    {
        public ProfileMapper()
        {
            CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();
            CreateMap<ShoppingCartItem, ShoppingCartItemResponse>().ReverseMap();

            CreateMap<CreateBasketCommand, ShoppingCart>()
    .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
    .ForMember(d => d.Items, o => o.MapFrom(s => s.Items))
    .ForMember(d => d.UserId, o => o.Ignore());

        }
    }
}
