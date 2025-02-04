using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyXchange.Controllers;
using CurrencyXchange.Models.AnalysisModel;
using CurrencyXchange.Models.CustomModels;
using CurrencyXchange.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace CurrencyXchange_UnitTest
{
    public class AnalyticsControler_UnitTest
    {
        private readonly Mock<IAnalyticsRepository> _mockRepo;
        private readonly AnalyticsController _controller;
        public AnalyticsControler_UnitTest()
        {
            _mockRepo = new Mock<IAnalyticsRepository>();
            _controller = new AnalyticsController(_mockRepo.Object);
        }

        [Fact]
        public async Task AverageUsersPnL_ReturnOkResponse_ReturnsCorrectValues()
        {
            DateTime startDate = DateTime.Now;
            var responseDto = new List<ProfitLossResult>() { new ProfitLossResult() { Amount = 100, Type = "USD", UserId = 1 } };
            _mockRepo.Setup(repo => repo.GetAveragePnl(startDate)).ReturnsAsync(responseDto);

            var result = await _controller.AverageUsersPnL(startDate);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProfitLossResult>>(okResult.Value);
            Assert.Equal(100, returnValue[0].Amount);
            Assert.Equal("USD", returnValue[0].Type);
            Assert.Equal(1, returnValue[0].UserId);
        }
    }
}
