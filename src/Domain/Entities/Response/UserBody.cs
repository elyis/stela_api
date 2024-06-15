using stela_api.src.Domain.Enums;

namespace stela_api.src.Domain.Entities.Response
{
    public class UserBody
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}