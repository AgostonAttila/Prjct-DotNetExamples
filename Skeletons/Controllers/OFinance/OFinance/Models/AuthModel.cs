namespace OFinance.API.Models
{
    public class AuthModel
    {
        public record LoginRequest(string Username, string Password);
        public record RegisterRequest(string Username, string Password, string Email);
        public record AuthResponse(string Token, string Username);
        public record ErrorResponse(string Message, string? Details = null);        
    }
}
