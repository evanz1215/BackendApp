using Infrastructure.Authentication.Core.Model;
using Infrastructure.Authentication.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WebAPI.Authentication.Services.Dtos;

namespace WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/account")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto login)
          => ProduceLoginResponse(
              await _userService.SignIn(login.Username, login.Password));

        private ActionResult<LoginResponseDto> ProduceLoginResponse((MySignInResult result, SignInData data) loginResults)
        {
            var (result, data) = loginResults;

            return result switch
            {
                MySignInResult.Failed => Unauthorized("Username or password incorrect."),
                MySignInResult.LockedOut => Forbid("User is temporarily locked out."),
                MySignInResult.NotAllowed => Forbid("User is not allowed to sign in."),
                MySignInResult.Success => Ok(new LoginResponseDto()
                {
                    AccessToken = data.Token.AccessToken,
                    TokenType = data.Token.TokenType,
                    ExpiresIn = data.Token.GetRemainingLifetimeSeconds(),
                    Username = data.Username,
                    Email = data.Email,
                    IsExternalLogin = data.IsExternalLogin,
                    ExternalAuthenticationProvider = data.ExternalAuthenticationProvider
                }),
                _ => throw new InvalidEnumArgumentException($"Unknown sign-in result '{result}'.")
            };
        }
    }
}