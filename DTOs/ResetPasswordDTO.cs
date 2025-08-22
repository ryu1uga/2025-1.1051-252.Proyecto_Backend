namespace Loop.DTOs.Common
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; } = string.Empty;
        public int Code { get; set; }
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
