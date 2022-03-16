using Infrastructure.Authentication.Core.Model;

namespace Infrastructure.Authentication.Core.Services
{
    public interface ITokenService
    {
        TokenModel CreateAuthenticationToken(Guid userId, string uniqueName, IEnumerable<(string claimType, string claimValue)> customClaims = null);
    }
}