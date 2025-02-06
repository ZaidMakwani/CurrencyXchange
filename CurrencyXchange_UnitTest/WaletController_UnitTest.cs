using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyXchange.Controllers;
using CurrencyXchange.Models.CustomModels;
using CurrencyXchange.Models.UserModels;
using CurrencyXchange.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CurrencyXchange_UnitTest
{
    public class WaletController_UnitTest
    {
        private readonly Mock<IWalletRepository> _mockrepo;
        private readonly WalletController _controller;

        public WaletController_UnitTest()
        {
            _mockrepo = new Mock<IWalletRepository>();
            _controller = new WalletController(_mockrepo.Object);
        }

        [Fact]
        public async Task GetCurrentBalance_ReturnsResultOk()
        {
            int userId = 1;
            var responseDto = new ResponseDto() { Message = "Current Balance is : ", Status = true };
            _mockrepo.Setup(repo => repo.GetCurrentBalance(userId)).ReturnsAsync(responseDto);

            var result = await _controller.GetCurrentBalance(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(returnValue.Status);
            Assert.Equal("Current Balance is : ", returnValue.Message);
        }

        [Fact]
        public async Task GetCurrentBalance_ReturnsNotFound()
        {
            int userId = 0;
            var responseDto = new ResponseDto { Status = false, Message = "Some Error Occured" };
            _mockrepo.Setup(repo => repo.GetCurrentBalance(userId)).ReturnsAsync(responseDto);

            var result = await _controller.GetCurrentBalance(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.False(returnValue.Status);
            Assert.Equal("Some Error Occured", returnValue.Message);
        }
    }
}
