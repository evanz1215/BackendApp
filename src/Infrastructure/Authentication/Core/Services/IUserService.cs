using Infrastructure.Authentication.Core.Model;

namespace Infrastructure.Authentication.Core.Services
{
    public interface IUserService
    {
        Task<(MySignInResult result, SignInData data)> SignIn(string username, string password);
    }
}