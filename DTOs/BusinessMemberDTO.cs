namespace Loop.DTOs.Common
{
    public class BusinessMemberDTO
    {
        public Guid UserId { get; set; }
        public Guid BusinessId { get; set; }
        public string Role { get; set; } = "";
    }
}