using CurrencyXchange.Models.CustomModels;
using CurrencyXchange.Models.UserModels;

namespace CurrencyXchange.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// The method is used to Generate a Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseDto> GenerateToken(UserLoginDto user);

        /// <summary>
        /// The Method is used to Register a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseDto> RegisterUser(RegisterUserDto user);
    }
}
