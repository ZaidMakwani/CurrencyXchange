using CurrencyXchange.Models.CustomModels;
using CurrencyXchange.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyXchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        public readonly IWalletRepository _walletRepository;


        public WalletController(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
            
        }

        [HttpGet]
        [Route("GetCurrentBalance")]
        public async Task<IActionResult> GetCurrentBalance(int UserId)
        {
            ResponseDto AvailableBalance = new ResponseDto();
            if (UserId != null)
            {
                AvailableBalance =await _walletRepository.GetCurrentBalance(UserId);
                return Ok(AvailableBalance);
            }
            else
            {
                return NotFound();
            }
        }
        //Add money to existing user's wallet
        [HttpPost]
        [Route("AddMoneyToWallet")]
        public async Task<IActionResult> AddMoneyToWallet(AddMoneyDto details)
        {
            ResponseDto status = await _walletRepository.AddMoneyToWallet(details);
            
            return status.Status? Ok(status):BadRequest();
        }

        //Create a new user's wallet
        [HttpPost]
        [Route("CreateUserWallet")]
        public async Task<IActionResult> CreateUserWallet(CreateWalletDto details)
        {
            ResponseDto response = new ResponseDto();

            response = await _walletRepository.CreateUserWallet(details);
            
            return response.Status? Ok(response):BadRequest();
        }

        //Upload a photo
        [HttpPost]
        [Route("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            ResponseDto response = new ResponseDto();

            response = await _walletRepository.UploadPhoto(file);

            return Ok(response);
        }


        //Make Transaction between users
        [HttpPost]
        [Route("TransferMoney")]
        public async Task<ResponseDto> TransferMoney(MoneyTransferDto credentials)
        {
            ResponseDto response = await _walletRepository.TransferMoney(credentials);
            return response;
        }
    }
}
