using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using CurrencyXchange.Models;
using CurrencyXchange.Models.CustomModels;
using CurrencyXchange.Models.UserModels;
using CurrencyXchange.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CurrencyXchange_UnitTest
{
    //The test case should be executed one after another because of the static data
    //The static data is being used because we wanted to mock the live database
    public class UsersRepository_UnitTest
    {
        private CurrencyXchangeContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CurrencyXchangeContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            return new CurrencyXchangeContext(options);
        }

        private IConfiguration GetMockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "a16byteslongkey!a16byteslongkey!" },
                { "Jwt:Issuer", "testIssuer" },
                { "Jwt:Audience", "testAudience" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private void SeedTestData()
        {
            var testUser = new User
            {
                UserId = 1,
                Name = "TestUser",
                Email = "test@example.com",
                Password = "password123",
                Address = "123 Test Street",
                Mobile = "9876543210",
                CurrencyId = 1,
                Currency = new Currency { Id = 1, Code = "USD", Name = "US Dollar" }
            };

            _context.Users.Add(testUser);
            _context.SaveChanges();
        }

        private readonly CurrencyXchangeContext _context;
        private readonly UsersRepository _repository;

        public UsersRepository_UnitTest()
        {
            _context = GetInMemoryDbContext();
            _repository = new UsersRepository(_context, GetMockConfiguration());
            SeedTestData();
        }

        [Fact]
        public async Task GenerateTokenForUser_ShouldReturnValidToken()
        {
            string token = await _repository.GenerateTokenForUser("TestUser", 1);

            Assert.False(string.IsNullOrEmpty(token));

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            Assert.NotNull(jsonToken);
            Assert.Contains("_userID", jsonToken.Claims.Select(c => c.Type));
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var userLoginDto = new UserLoginDto
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var response = await _repository.GenerateToken(userLoginDto);

            Assert.True(response.Status);
            Assert.False(string.IsNullOrEmpty(response.Message));
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnError_WhenCredentialsAreInvalid()
        {
            var userLoginDto = new UserLoginDto
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var response = await _repository.GenerateToken(userLoginDto);

            Assert.False(response.Status);
            Assert.Equal("Invalid Credenetials", response.Message);
        }

        [Fact]
        public async Task RegisterUser_ShouldSucceed_WhenUserIsCreated()
        {
            var newUser = new RegisterUserDto
            {
                UserName = "NewUser",
                Email = "newuser@example.com",
                Password = "pass123",
                Address = "123 Test Street",
                Mobile = "1234567890",
                CurrencyId = 1
            };

            var response = await _repository.RegisterUser(newUser);

            Assert.True(response.Status);
            Assert.Equal("User Created Successfully", response.Message);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnError_WhenSaveFails()
        {
            var newUser = new RegisterUserDto
            {
                UserName = "ErrorUser",
                Email = "error@example.com",
                Password = "pass123",
                Address = "123 Error Street",
                Mobile = "1234567890",
                CurrencyId = 1
            };

            _context.Database.EnsureDeleted();

            var response = await _repository.RegisterUser(newUser);

            Assert.False(response.Status);
            Assert.Equal("Some Error Occured while Creating the user", response.Message);
        }
    }
}
