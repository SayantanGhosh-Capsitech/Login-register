using Login_register.Models;
using Login_register.Dtos;

namespace Login_register.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, string Message, string Token, User? user)> LoginAsync(LoginDto dto);
    }
}
