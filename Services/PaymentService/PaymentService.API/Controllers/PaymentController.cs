using EventBus.Abstractions;
using EventBus.Events.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.API.Applications.Services;
using System.Net;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;

namespace PaymentService.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IVnpay _vnpay;
        private IConfiguration _configuration;
        private IPaymentService _paymentService;
        private IEventBus _eventBus;
        public PaymentController(IVnpay vnpay, IConfiguration configuration, IPaymentService paymentService, IEventBus eventBus)
        {
            _configuration = configuration;
            _vnpay = vnpay;
            _vnpay.Initialize(_configuration["VNPay:TmnCode"], _configuration["VNPay:HashSecret"], _configuration["VNPay:BaseUrl"], _configuration["VNPay:ReturnUrl"]);
            _paymentService = paymentService;
            _eventBus = eventBus;
        }

        [HttpGet("amount/{amount}")]
        public IActionResult PayOrder(int amount)
        {
            //var ipAddr = _configuration["VNPay:IpAddr"];
            //var request = new PaymentRequest
            //{
            //    PaymentId = DateTime.Now.Ticks,
            //    Money = amount,
            //    Description = "Thanh toan don hang",
            //    IpAddress = ipAddr,
            //    BankCode = BankCode.VNBANK, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
            //    CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
            //    Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
            //    Language = DisplayLanguage.Vietnamese // Tùy chọn. Mặc định là tiếng Việt
            //};
            //var payUrl = _vnpay.GetPaymentUrl(request);
            var payUrl = _paymentService.GeneratePaymentUrl(amount, $"Thanh toan don hang {Guid.NewGuid()}");
            return Ok(payUrl);
        }

        [HttpGet("vnpay/callback")]
        public async Task<IActionResult> VNPayCallback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);
                    Console.WriteLine($"Query: {Request.QueryString.Value}");
                    if (paymentResult.IsSuccess)
                    {
                        var @event = new PaymentSuccessfulEvent
                        {
                            RelatedPayEventId = Guid.Parse(paymentResult.Description.Split(' ').Last())
                        };
                        await _eventBus.PublishEventAsync(@event);
                        var successfulRedirectUri = _configuration["VNPay:PaymentSuccessfulRedirect"];
                        return Redirect(successfulRedirectUri);
                    }
                    var failedRedirectUri = _configuration["VNPay:PaymentFailedRedirect"];
                    return Redirect(failedRedirectUri);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}
