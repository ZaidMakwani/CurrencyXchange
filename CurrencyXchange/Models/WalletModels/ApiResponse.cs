using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CurrencyXchange.Models.CustomModels
{
    public class ApiResponse
    {
        public string Date { get; set; }
        public string Historical { get; set; }
        public Info Info { get; set; }
        public Query Query { get; set; }
        public decimal result { get; set; }
        public bool Success { get; set; }

    }
}
