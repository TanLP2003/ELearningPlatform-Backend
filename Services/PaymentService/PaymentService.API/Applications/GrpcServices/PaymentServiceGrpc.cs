using Grpc.Core;
using PaymentService.API.Applications.Services;
using PaymentService.API.Protos;

namespace PaymentService.API.Applications.GrpcServices
{
    public class PaymentServiceGrpc(IPaymentService paymentService, ILogger<PaymentServiceGrpc> logger) : PaymentServiceProtoGrpc.PaymentServiceProtoGrpcBase
    {
        public override Task<PaymentData> GeneratePayUrl(PayRequest request, ServerCallContext context)
        {
            var orderInfo = $"Thanh toan don hang {request.PayEventId}";
            var amount = request.Amount;

            logger.LogInformation($"========> PayEventId: {request.PayEventId}");

            var payUrl = paymentService.GeneratePaymentUrl(amount, orderInfo);
            return Task.FromResult(new PaymentData
            {
                PayUrl = payUrl
            });
        }
    }
}
