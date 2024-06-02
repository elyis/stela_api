namespace webApiTemplate.src.Domain.Entities.Shared
{
    public class TokenPayload
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }
}