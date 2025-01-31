namespace Auth.API.Models
{
    public class AuthUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public AuthRefreshToken RefreshToken { get; set; }
    }
}
