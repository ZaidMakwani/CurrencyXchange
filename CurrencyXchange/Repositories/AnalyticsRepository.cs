using CurrencyXchange.Models;
using CurrencyXchange.Models.AnalysisModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace CurrencyXchange.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        public readonly CurrencyXchangeContext _context;

        public AnalyticsRepository(CurrencyXchangeContext context)
        {
            _context = context;
        }

        public async Task<List<ProfitLossResult>> GetAveragePnl(DateTime startDate)
        {
            var result = await _context.Database.SqlQuery<ProfitLossResult>($"EXEC GetUserProfitOrLoss @StartDate = {startDate}").ToListAsync();

            return result;
        }

    }
}
