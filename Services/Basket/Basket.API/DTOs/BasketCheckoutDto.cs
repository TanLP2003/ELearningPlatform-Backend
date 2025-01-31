namespace Basket.API.DTOs
{
    public class BasketCheckoutDto
    {
        public string UserName { get; set; } = default!;
        public string CardName { get; set; } = default!;
        public string CardNumber { get; set; } = default!;
        public string Expiration { get; set; } = default!;
        public string CVV { get; set; } = default!;
    }
}
