namespace PaymentService.API.Applications.Services
{
    public interface IPaymentService
    {
        string GeneratePaymentUrl(int amount, string orderInfo);
    }
}
