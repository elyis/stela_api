using System.ComponentModel.DataAnnotations;
using stela_api.src.Domain.Enums;

namespace stela_api.src.Domain.Entities.Request
{
    public class UserPatchBody
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        [EnumDataType(typeof(UserRole))]
        public UserRole? Role { get; set; }
    }
}