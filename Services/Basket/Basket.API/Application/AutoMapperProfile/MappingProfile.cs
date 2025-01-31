using AutoMapper;
using Basket.API.Application.Commands.BuyNow;
using Basket.API.Application.Commands.CheckoutBasket;
using Basket.API.DTOs;
using Basket.API.Models;
using EventBus.Events;

namespace Basket.API.Application.AutoMapperProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartItem, CartItemDto>().ReverseMap();
        //CreateMap<CheckoutBasketCommand, BasketCheckoutDto>().ReverseMap();
        //CreateMap<BuyNowCommand, BasketCheckoutDto>().ReverseMap();
        //CreateMap<CheckoutBasketCommand, BasketCheckoutedEvent>().ReverseMap();
        CreateMap<CartItem, BasketItemCheckoutedEvent>().ReverseMap();
    }
}