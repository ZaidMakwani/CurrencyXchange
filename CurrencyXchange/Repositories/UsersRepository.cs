using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using CurrencyXchange.Models;
using CurrencyXchange.Models.CustomModels;
using CurrencyXchange.Models.UserModels;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyXchange.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly CurrencyXchangeContext _context;
        private readonly IConfiguration _configuration;

        public UsersRepository(CurrencyXchangeContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> GenerateTokenForUser(string username, int UserId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("_userID", UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(540),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<ResponseDto> GenerateToken(UserLoginDto user)
        {

            User loggedinUser = await _context.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstAsync();
            if (loggedinUser == null)
            {
                return new ResponseDto() { Status = false, Message = "Invalid Credenetials" };
            }
            string Token = await GenerateTokenForUser(loggedinUser.Name, loggedinUser.UserId);
            return new ResponseDto() { Status = true, Message = Token };
        }

        public async Task<ResponseDto> RegisterUser(RegisterUserDto user)
        {
            ResponseDto response = new ResponseDto();

            User newUser = new User()
            {
                Address = user.Address,
                CurrencyId = user.CurrencyId,
                Email = user.Email,
                Mobile = user.Mobile,
                Name = user.UserName,
                Password  = user.Password,
            };

            try
            {
                _context.Users.Add(newUser);
                int result = await _context.SaveChangesAsync();
                if(result == 0)
                {
                    return new ResponseDto { Message = "Some Error Occured while Creating the user", Status = false };
                }
            }
            catch(Exception e)
            {
                return new ResponseDto { Message = "Some Error Occured while Creating the user", Status = false };
            }


            response.Status = true;
            response.Message = "User Created Successfully";
            return response;

        }
    }
}
