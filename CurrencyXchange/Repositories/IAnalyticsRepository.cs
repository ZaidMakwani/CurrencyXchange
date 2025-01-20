using CurrencyXchange.Models.AnalysisModel;

namespace CurrencyXchange.Repositories
{
    public interface IAnalyticsRepository
    {
        Task<List<ProfitLossResult>> GetAveragePnl(DateTime startDate);
    }
}
