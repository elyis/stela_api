using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using stela_api.src.Domain.Entities.Shared;
using Microsoft.IdentityModel.Tokens;
using webApiTemplate.src.App.IService;
using webApiTemplate.src.Domain.Entities.Config;
using webApiTemplate.src.Domain.Entities.Shared;

namespace webApiTemplate.src.App.Service
{
    public class JwtService : IJwtService
    {
        private readonly SigningCredentials _signingCredentials;

        public JwtService(JwtSettings jwtSettings)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private string GenerateAccessToken(Dictionary<string, string> claims, TimeSpan timeSpan)
        {
            var tokenClaims = claims.Select(claim => new Claim(claim.Key, claim.Value));

            var token = new JwtSecurityToken(
                claims: tokenClaims,
                expires: DateTime.UtcNow.Add(timeSpan),
                signingCredentials: _signingCredentials
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken() => Guid.NewGuid().ToString();

        public TokenPair GenerateDefaultTokenPair(TokenPayload tokenPayload)
        {
            var claims = new Dictionary<string, string>{
                { "UserId", tokenPayload.UserId.ToString() },
                { ClaimTypes.Role, tokenPayload.Role},
            };
            var timeSpan = new TimeSpan(2, 0, 0, 0);
            return GenerateTokenPair(claims, timeSpan);
        }

        private TokenPair GenerateTokenPair(Dictionary<string, string> claims, TimeSpan timeSpan) =>
            new(
                    GenerateAccessToken(claims, timeSpan),
                    GenerateRefreshToken()
                );

        private List<Claim> GetClaims(string token) =>
            new JwtSecurityTokenHandler()
                .ReadJwtToken(token.Replace("Bearer ", ""))
                .Claims
                .ToList();

        public TokenPayload GetTokenPayload(string token)
        {
            var claims = GetClaims(token);
            return new TokenPayload
            {
                Role = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value,
                UserId = Guid.Parse(claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value),
            };
        }
    }
}