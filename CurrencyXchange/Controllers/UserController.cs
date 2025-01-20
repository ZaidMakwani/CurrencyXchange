using CurrencyXchange.Models.CustomModels;
using CurrencyXchange.Models.UserModels;
using CurrencyXchange.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace CurrencyXchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserRepository _repo;
        public UserController(IUserRepository repo)
        {
            _repo = repo;   
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            ResponseDto response = await _repo.GenerateToken(user);

            if(response.Status)
            {
                return Ok(response);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUserDto user)
        {
            ResponseDto response = await _repo.RegisterUser(user);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
