using CurrencyXchange.Models.CustomModels;

namespace CurrencyXchange.Repositories
{
    public interface IWalletRepository
    {
        Task<ResponseDto> AddMoneyToWallet(AddMoneyDto details);
        Task<ResponseDto> CreateUserWallet(CreateWalletDto details);
        Task<ResponseDto> GetCurrentBalance(int userId);
        Task<ResponseDto> TransferMoney(MoneyTransferDto credentials);
        Task<ResponseDto> UploadPhoto(IFormFile file);
    }
}
