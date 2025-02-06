using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyXchange.Models;
using CurrencyXchange.Models.CustomModels;
using CurrencyXchange.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CurrencyXchange_UnitTest
{
    public class WalletRepository_UnitTest
    {
        #region Creating an inmemory database
        private CurrencyXchangeContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CurrencyXchangeContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var context = new CurrencyXchangeContext(options);

            return context;
        }
        #endregion

        #region Mocking the configuration 
        private IConfiguration GetMockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "ApiLayer:ApiKey", "your_test_api_key" } // Mock API key for testing
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
        #endregion 

        #region Adding static data
        private void SeedTestData()
        {
            var testUser = new User
            {
                UserId = 1,
                CurrencyId = 1,
                Name = "TestUser1",
                Address = "TestAddress",
                Currency = new Currency { Id = 1, Code = "USD", Name = "US Dollar" }
            };

            var testWallet = new Wallet
            {
                Id = 1,
                UserId = 1,
                Balance = 10000
            };

            _context.Users.Add(testUser);
            _context.Wallets.Add(testWallet);
            _context.SaveChanges();
        }
        #endregion

        private readonly CurrencyXchangeContext _context;
        private readonly WalletRepository _repository;

        public WalletRepository_UnitTest()
        {
            _context = GetInMemoryDbContext();
            _repository = new WalletRepository(_context, GetMockConfiguration());

            // Seed some initial test data
            SeedTestData();
        }


        //This Method cannot be tested as it is using transaction and UseSqlServer is not 
        //capable of handling an transaction inside the method
        #region Unit test for Transfering Money
        //[Fact]
        //public async Task TransferMoney_ShouldSucceed_WhenFundsAreAvailable()
        //{
        //    // Arrange: Create a second user and wallet
        //    var receiver = new User
        //    {
        //        UserId = 2,
        //        Name = "TestUser2",
        //        CurrencyId = 1,
        //        Address = "TestAdd",
        //        Currency = new Currency { Id = 2, Code = "USD", Name = "US Dollar" }
        //    };


        //    _context.Users.Add(receiver);
        //    _context.SaveChanges();

        //    var transferRequest = new CreateWalletDto
        //    {
        //        UserId = 2,
        //        Amount = 2000
        //    };

        //    // Act
        //    var result = await _repository.CreateUserWallet(transferRequest);

        //    // Assert
        //    Assert.True(result.Status);
        //    Assert.Equal("Wallet created Successfully", result.Message);

        //    // Verify balances updated
        //    var senderWallet = await _context.Wallets.FirstAsync(w => w.UserId == 1);
        //    var updatedReceiverWallet = await _context.Wallets.FirstAsync(w => w.UserId == 2);

        //    Assert.Equal(8000, senderWallet.Balance);   // 1000 - 200
        //    Assert.Equal(7000, updatedReceiverWallet.Balance); // 500 + 200
        //}
        #endregion

        [Fact]
        public async Task CreateUserWallet_ShouldSucceed_WhenWalletCreated()
        {
            // Arrange: Create a second user and wallet
            var newUser = new User
            {
                UserId = 2,
                Name = "TestUser2",
                CurrencyId = 1,
                Address = "TestAdd",
                Currency = new Currency { Id = 2, Code = "USD", Name = "US Dollar" }
            };


            _context.Users.Add(newUser);
            _context.SaveChanges();

            var newWallet = new CreateWalletDto
            {
                UserId = 2,
                Amount = 2000
            };

            // Act
            var result = await _repository.CreateUserWallet(newWallet);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Wallet created Successfully", result.Message);

        }


    }
}
