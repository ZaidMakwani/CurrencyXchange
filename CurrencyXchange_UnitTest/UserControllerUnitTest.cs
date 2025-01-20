using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyXchange.Controllers;
using CurrencyXchange.Models.CustomModels;
using CurrencyXchange.Models.UserModels;
using CurrencyXchange.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CurrencyXchange_UnitTest
{
    public  class UserControllerUnitTest
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UserController _controller;

        public UserControllerUnitTest()
        {
            _mockRepo = new Mock<IUserRepository>();
            _controller = new UserController(_mockRepo.Object);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenTokenGeneratedSuccessfully()
        {
            var userLoginDto = new UserLoginDto { Email = "Test@gmail.com", Password = "Test@123" };
            var responseDto = new ResponseDto { Status = true, Message = "Success"};
            _mockRepo.Setup(repo => repo.GenerateToken(userLoginDto)).ReturnsAsync(responseDto);

            var result = await _controller.Login(userLoginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(returnValue.Status);
            Assert.Equal("Success", returnValue.Message);
        }


        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenTokenGenerationFails()
        {
            var userLoginDto = new UserLoginDto { Email = "testuser", Password = "wrongpassword" };
            var responseDto = new ResponseDto { Status = false, Message = "Invalid credentials"};
            _mockRepo.Setup(repo => repo.GenerateToken(userLoginDto)).ReturnsAsync(responseDto);

            var result = await _controller.Login(userLoginDto);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task RegisterUser_ReturnsOk_WhenUserRegistered()
        {
            var registerUserDto = new RegisterUserDto { Address = "testAddress", CurrencyId = 1, Email = "TestEmail", Mobile = "1236723232232", Password = "TestPassword", UserName = "Test" };
            var responseDto = new ResponseDto { Status = true, Message = "User Created Successfully" };
            _mockRepo.Setup(repo => repo.RegisterUser(registerUserDto)).ReturnsAsync(responseDto);

            var result = await _controller.Register(registerUserDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(returnValue.Status);
            Assert.Equal("User Created Successfully", returnValue.Message);
        }

    }
}
