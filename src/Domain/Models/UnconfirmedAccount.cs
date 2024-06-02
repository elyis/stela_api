using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace stela_api.src.Domain.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class UnconfirmedAccount
    {
        public Guid Id { get; set; }

        [StringLength(256, MinimumLength = 3)]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string? ConfirmationCode { get; set; }
        public DateTime? ConfirmationCodeValidBefore { get; set; }

        public Account ToAccount()
        {
            return new Account
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                PasswordHash = PasswordHash,
            };
        }
    }
}