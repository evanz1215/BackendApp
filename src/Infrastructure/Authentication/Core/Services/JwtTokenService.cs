using Infrastructure.Authentication.Core.Model;
using Infrastructure.Authentication.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Authentication.Core.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly AuthenticationSettings _authSettings;

        public JwtTokenService(AuthenticationSettings authSettings)
        {
            _authSettings = authSettings;
        }

        public TokenModel CreateAuthenticationToken(Guid userId, string uniqueName,
            IEnumerable<(string claimType, string claimValue)> customClaims = null)
        {
            var expiration = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Iss, _authSettings.JwtIssuer),
                    new Claim(JwtRegisteredClaimNames.Aud, _authSettings.JwtAudience),
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName)
                }.Concat(
                    customClaims?.Select(x => new Claim(x.claimType, x.claimValue)) ?? Enumerable.Empty<Claim>())
                ),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_authSettings.JwtSigningKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new TokenModel(
                tokenType: "Bearer",
                accessToken: tokenString,
                expiresAt: expiration
            );
        }
    }
}