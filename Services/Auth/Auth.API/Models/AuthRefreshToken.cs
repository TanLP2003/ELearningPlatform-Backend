namespace Auth.API.Models
{
    public class AuthRefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }

        public Guid UserId { get; set; }

        public DateTime ExpiredAt { get; set; }
    }
}
