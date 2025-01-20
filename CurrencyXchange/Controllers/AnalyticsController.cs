using CurrencyXchange.Models.AnalysisModel;
using CurrencyXchange.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyXchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsRepository _repo;
        public AnalyticsController(IAnalyticsRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Route("PNLUsers")]
        public async Task<IActionResult> AverageUsersPnL(DateTime startDate)
        {
            List<ProfitLossResult> response = await _repo.GetAveragePnl(startDate);

            return Ok(response);
        }
    }
}
