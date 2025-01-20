namespace CurrencyXchange.Models.CustomModels
{
    public class AddMoneyDto
    {
        public int UserId { get; set; }
        public string CurrencyType { get; set; }
        public long Amount { get; set; }
        public string TransactionType { get; set; }
    }
}
