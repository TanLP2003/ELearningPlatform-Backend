using AutoMapper;
using Basket.API.Application.Commands.AddCartItem;
using Basket.API.Application.Commands.BuyNow;
using Basket.API.Application.Commands.CheckoutBasket;
using Basket.API.Application.Commands.EmptyBasket;
using Basket.API.Application.Commands.RemoveItem;
using Basket.API.Application.Queries.GetBasket;
using Basket.API.DTOs;
using Basket.API.Models;
using Basket.API.Protos;
using Basket.API.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Basket.API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController(ICartRepository cartRepository, 
        ISender sender, 
        IMapper mapper, 
        PaymentServiceProtoGrpc.PaymentServiceProtoGrpcClient paymentServiceClient
        ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCartForUser()
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            var newGetBasketQuery = new GetBasketQuery(Guid.Parse(userId));
            var result = await sender.Send(newGetBasketQuery);
            return Ok(result);
        }
        //[HttpPost("{userId}")]
        //public async Task<IActionResult> CreateCart(Guid userId)
        //{
        //    var newCart = new Cart(userId);
        //    await cartRepository.CreateCart(newCart);
        //    return Ok();
        //}
        [HttpPut("add-item")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemDto item)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            var newItem = mapper.Map<CartItem>(item);
            var addCartItemCommand = new AddCartItemCommand(Guid.Parse(userId), newItem);
            var result = await sender.Send(addCartItemCommand);
            return Ok(result);
        }
        [HttpPut("remove-item/{itemId}")]
        public async Task<IActionResult> RemoveCartItem(Guid itemId)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            var newRemoveItemCommand = new RemoveItemCommand(Guid.Parse(userId), itemId);
            var result = await sender.Send(newRemoveItemCommand);   
            return Ok(result);
        }

        //[HttpPut("/clean-basket/{userId}")]
        //public async Task<IActionResult> CleanBasket(Guid userId)
        //{
        //    var newCleanBasketCommand = new CleanBasketCommand(userId);
        //    var result = await sender.Send(newCleanBasketCommand);
        //    return Ok(result);  
        //}
        [HttpPost("checkout-basket")]
        public async Task<IActionResult> CheckoutBasket()
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            //var newCheckoutBasketCommand = mapper.Map<CheckoutBasketCommand>(basketCheckout);
            var newCheckoutBasketCommand = new CheckoutBasketCommand { UserId = Guid.Parse(userId) };
            //newCheckoutBasketCommand.UserId = Guid.Parse(userId);
            var paymentUrl = await sender.Send(newCheckoutBasketCommand);
            return Ok(new { RedirectUrl = paymentUrl });
        }

        [HttpPost("buynow/{courseId}")]
        public async Task<IActionResult> BuyNow(Guid courseId)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            //var newBuyNowCommand = mapper.Map<BuyNowCommand>(formBuynow);
            var newBuyNowCommand = new BuyNowCommand
            {
                UserId = Guid.Parse(userId),
                CourseId = courseId
            };
            var paymentUrl = await sender.Send(newBuyNowCommand);
            return Ok(new { RedirectUrl = paymentUrl });
        }
    }
}