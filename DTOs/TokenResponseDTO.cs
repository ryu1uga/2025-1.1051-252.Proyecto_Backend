namespace Loop.DTOs.Common
{
    public class TokenResponseDTO
    {
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public Guid UserId { get; set; }
    }
}