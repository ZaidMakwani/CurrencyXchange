using System.Text.Json;
using CurrencyXchange.Models;
using CurrencyXchange.Models.CustomModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CurrencyXchange.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        public CurrencyXchangeContext _context;
        public IConfiguration _config;
        private readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");


        public WalletRepository(CurrencyXchangeContext context, IConfiguration config)
        {
            _context = context;
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
            _config = config;
        }

        public async Task<ResponseDto> AddMoneyToWallet(AddMoneyDto details)
        {
            UserTransaction transaction = new UserTransaction();
            ResponseDto response = new ResponseDto();
            //To better optimize this we can perform the three operations in a procedure diretly
            //We'll look into this later
            #region Fetching the required credentials
            Wallet wallet = _context.Wallets.Where(u => u.UserId == details.UserId).First();

            User user = _context.Users.Where(u => u.UserId == details.UserId).First();

            user.Currency = _context.Currencies.Where(u => u.Id == user.CurrencyId).First();
            #endregion


            transaction.UserId = details.UserId;
            transaction.WalletId = wallet.Id;
            transaction.CurrencyId = user.CurrencyId;
            transaction.Time = DateTime.Now;


            if (wallet != null)
            {
                decimal ExchangedAmount = 0;
                if (user.Currency.Code == details.CurrencyType)
                {
                    ExchangedAmount = details.Amount;
                }
                else
                {
                    ConversionResponse exchangeRate = await GetExchangeRate(details.CurrencyType, user.Currency.Code, details.Amount);
                    if (exchangeRate.status)
                    {
                        ExchangedAmount = exchangeRate.Amount;
                    }
                    else
                    {
                        return new ResponseDto { Message = "Some Erro OCcured while converting the currency", Status = false };
                    }
                }
                transaction.Amount = ExchangedAmount;

                #region Checking the Transaction type and updating the wallet
                if (details.TransactionType == "Credit")
                {
                    wallet.Balance += ExchangedAmount;
                    _context.Update(wallet);
                    transaction.Balance = wallet.Balance;
                    transaction.TransactionType = "Credit";
                }
                else if (details.TransactionType == "Debit")
                {
                    if (wallet.Balance >= ExchangedAmount)
                    {
                        wallet.Balance -= ExchangedAmount;
                        _context.Update(wallet);
                        transaction.Balance = wallet.Balance;
                        transaction.TransactionType = "Debit";
                    }
                    else
                    {
                        return new ResponseDto { Message = "The wallet does not have enough money for the transfer", Status = false };
                    }
                }
                #endregion

                try
                {
                    _context.SaveChanges();
                    bool TransactionStatus = await UpdateTransaction(transaction);

                    if (TransactionStatus)
                    {
                        response.Status = true;
                        response.Message = "Successfully added money to the wallet";
                        return response;
                    }
                    else
                    {
                        response.Status = false;
                        response.Message = "Some error occured while making transaction.";
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Status = false;
                    response.Message = "Some error occured while adding money to the wallet";
                    return response;
                }
            }
            else
            {
                response.Status = false;
                response.Message = "Some error occured while adding money to the wallet";
                return response;
            }
        }

        //Change this to add a get a new parameter
        public async Task<ConversionResponse> GetExchangeRate(string FromCurr, string ToCurr, long Amount)
        {
            ConversionResponse res = new ConversionResponse();
            string to = ToCurr;
            string from = FromCurr;
            string cuurAmount = Amount.ToString();
            //Add the API key in the appstettings.json 
            string apiKey = _config["ApiLayer:ApiKey"];
            string url = $"https://api.apilayer.com/exchangerates_data/convert?to={to}&from={from}&amount={cuurAmount}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apikey", apiKey);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        var jsonResponse = JsonSerializer.Deserialize<ApiResponse>(content);

                        res.status = true;
                        res.Amount = jsonResponse.result;
                        return res;
                    }
                    else
                    {
                        return new ConversionResponse { Amount = 0, status = false };
                    }
                }
                catch (Exception ex)
                {
                    return new ConversionResponse { Amount = 0, status = false };
                }
            }
        }

        public async Task<ResponseDto> GetCurrentBalance(int userId)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                decimal? balance = await _context.Wallets.Where(u => u.UserId == userId).Select(u => u.Balance).FirstAsync();
                response.Status = true;
                response.Message = "Current Balance is : " + balance;
            }
            catch (Exception e)
            {
                response.Status = false;
                response.Message = "Some Error Occured";
            }
            return response;
        }

        public async Task<ResponseDto> CreateUserWallet(CreateWalletDto details)
        {
            ResponseDto response = new ResponseDto();

            UserTransaction transaction = new UserTransaction();
            User user = new User();
            #region Check if the user Exists
            try
            {
                user = _context.Users.Where(u => u.UserId == details.UserId).First();
                if (user == null)
                {
                    response.Status = false;
                    response.Message = "NO such user Exists";
                }
                else
                {
                    transaction.UserId = user.UserId;
                    transaction.CurrencyId = user.CurrencyId;
                    transaction.Amount = details.Amount;
                    //Whem creating a wallet the user will always add money first
                    transaction.TransactionType = "Credit";
                    transaction.Time = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                response.Status = false;
                response.Message = "NO such user Exists";
                return response;
            }
            #endregion

            #region Create a New wallet
            Wallet newWallet = new Wallet() { Balance = details.Amount, UserId = details.UserId };
            try
            {

                _context.Wallets.Add(newWallet);
                int WalletCreated = _context.SaveChanges();

                if (WalletCreated > 0)
                {
                    int walletID = _context.Wallets.Where(u => u.UserId == details.UserId).Select(u => u.Id).First();
                    transaction.WalletId = walletID;
                    transaction.Balance = details.Amount;

                    //When addition to the wallet completed add the transaction in the
                    bool TransactionStatus = await UpdateTransaction(transaction);
                }
                else
                {
                    response.Status = false;
                    response.Message = "Some Error Occured when creating a new wallet";
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Status = false;
                response.Message = "Some Error Occured when creating a new wallet";
                return response;
            }
            #endregion

            response.Status = true;
            response.Message = "Wallet created Successfully";

            return response;

        }

        public async Task<ResponseDto> UploadPhoto(IFormFile file)
        {
            ResponseDto response = new ResponseDto();
            //Can also ask for user id and update the file's path in the database to be able to access the photo
            if (file == null || file.Length == 0)
            {
                return new ResponseDto() { Message = "No file was provided.", Status = false };
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return new ResponseDto() { Message = "Invalid file type. Only JPG and PNG are allowed.", Status = false };
            }

            var fileName = Guid.NewGuid() + fileExtension;

            var filePath = Path.Combine(_uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new ResponseDto { Status = true, Message = "File uploaded successfully." };
        }

        //Write a function to update the transaction log
        public async Task<bool> UpdateTransaction(UserTransaction transactionDetails)
        {
            try
            {
                await _context.UserTransactions.AddAsync(transactionDetails);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }

        //The API call is taking too long take some steps to reduce the time interval for this
        public async Task<ResponseDto> TransferMoney(MoneyTransferDto transferDetails)
        {
            ResponseDto response = new ResponseDto();
            ResponseDto receiversResponse = new ResponseDto();
            ResponseDto sendersResponse = new ResponseDto();

            //Fetch the transfering currency
            #region Fetching the Sender's Currency
            string currencyType = (from a in _context.Users
                                   from b in _context.Currencies
                                   where a.CurrencyId == b.Id &&
                                   a.UserId == transferDetails.SendersId
                                   select b.Code
                                ).First();
            #endregion

            #region Creating Object for sender and receiver
            AddMoneyDto sendersCred = new AddMoneyDto()
            {
                UserId = transferDetails.SendersId,
                Amount = transferDetails.Amount,
                CurrencyType = currencyType,
                TransactionType = "Debit"
            };
            AddMoneyDto receiversCred = new AddMoneyDto()
            {
                UserId = transferDetails.ReceiversId,
                Amount = transferDetails.Amount,
                CurrencyType = currencyType,
                TransactionType = "Credit"
            };
            #endregion

            //Create a transaction for both of these actions
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    try
                    {
                        sendersResponse = await AddMoneyToWallet(sendersCred);
                    }
                    catch (Exception ex)
                    {
                        response = new ResponseDto() { Status = false, Message = "Some Error Occured when Sending Money " };
                        throw new Exception(response.Message);
                    }
                    if(sendersResponse.Status == false)
                    {
                        throw new Exception(sendersResponse.Message);
                    }
                    try
                    {
                        receiversResponse = await AddMoneyToWallet(receiversCred);
                    }
                    catch (Exception ex)
                    {
                        response = new ResponseDto() { Message = "Some Error Occured while Receving Money", Status = false };
                        throw new Exception(response.Message);
                    }
                    if (receiversResponse.Status == false)
                    {
                        throw new Exception(receiversResponse.Message);
                    }
                    else
                    {
                        transaction.Commit();
                    }
                    return new ResponseDto { Message = "Transaction Completed Successfully", Status = true };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new ResponseDto() { Message = e.Message, Status = false };

                }
            }
            
            
        }



    }
}
