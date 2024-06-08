using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Request;
using stela_api.src.Domain.Entities.Shared;
using stela_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using webApiTemplate.src.App.IService;
using webApiTemplate.src.App.Provider;
using webApiTemplate.src.Domain.Entities.Shared;
using stela_api.src.Domain.Entities.Shared.Utility;
using stela_api.src.Domain.Enums;

namespace stela_api.src.App.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnconfirmedAccountRepository _unconfirmedAccountRepository;
        private readonly IBusketRepository _busketRepository;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthService
        (
            IAccountRepository accountRepository,
            IUnconfirmedAccountRepository unconfirmedAccountRepository,
            IBusketRepository busketRepository,
            IJwtService jwtService,
            IEmailService emailService
        )
        {
            _accountRepository = accountRepository;
            _busketRepository = busketRepository;
            _unconfirmedAccountRepository = unconfirmedAccountRepository;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<IActionResult> ApplyForRegistration(SignUpBody body)
        {
            var account = await _accountRepository.GetByEmail(body.Email);
            if (account != null)
                return new ConflictResult();

            var code = CodeGenerator.Generate();
            await _unconfirmedAccountRepository.Remove(body.Email);
            await _unconfirmedAccountRepository.AddAsync(body, code);

            var isMessageSended = await _emailService.SendMessage(body.Email, "Confirm account", $"Confirmation code: {code}");
            return isMessageSended ? new OkResult() : new BadRequestResult();
        }

        public async Task<IActionResult> RestoreToken(string refreshToken)
        {
            var user = await _accountRepository.GetByTokenAsync(refreshToken);
            if (user == null)
                return new NotFoundResult();

            var tokenPair = await UpdateToken(user.RoleName, user.Id);
            return new OkObjectResult(tokenPair);
        }

        public async Task<IActionResult> SignIn(SignInBody body)
        {
            var user = await _accountRepository.GetByEmail(body.Email);
            if (user == null)
                return new NotFoundResult();

            var inputPasswordHash = Hmac512Provider.Compute(body.Password);
            if (user.PasswordHash != inputPasswordHash)
                return new BadRequestResult();

            var tokenPair = await UpdateToken(user.RoleName, user.Id);
            return new OkObjectResult(tokenPair);
        }

        public async Task<IActionResult> VerifyUnconfirmedAccount(string email, string code)
        {
            var account = await _unconfirmedAccountRepository.GetByEmail(email);
            if (account == null)
                return new NotFoundResult();

            if (account.ConfirmationCode != code)
                return new BadRequestObjectResult("Invalid confirmation code");

            if (account.ConfirmationCodeValidBefore < DateTime.UtcNow)
                return new BadRequestObjectResult("Confirmation period has expired");

            var body = new SignUpBody
            {
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Password = account.PasswordHash,
            };

            var rolename = UserRole.User.ToString();
            var result = await _accountRepository.AddAsync(body, rolename);

            var tokenPair = await UpdateToken(rolename, result.Id);
            var busket = await _busketRepository.CreateBusket(result);
            return new OkObjectResult(tokenPair);
        }

        private async Task<TokenPair> UpdateToken(string rolename, Guid userId)
        {
            var tokenInfo = new TokenPayload
            {
                Role = rolename,
                UserId = userId
            };

            var tokenPair = _jwtService.GenerateDefaultTokenPair(tokenInfo);
            tokenPair.RefreshToken = await _accountRepository.UpdateTokenAsync(tokenPair.RefreshToken, tokenInfo.UserId);
            tokenPair.Role = Enum.Parse<UserRole>(rolename);
            return tokenPair;
        }
    }
}