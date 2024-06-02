using stela_api.src.Domain.Enums;

namespace stela_api.src.Domain.Entities.Response
{
    public class ProfileBody
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }
        public string? UrlImage { get; set; }
    }
}