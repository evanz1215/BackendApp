using System.ComponentModel.DataAnnotations;

namespace WebAPI.Authentication.Services.Dtos
{
    public record LoginDto
    {
        [Required, MinLength(1)]
        public string Username { get; init; }

        [Required, MinLength(1)]
        public string Password { get; init; }
    }
}