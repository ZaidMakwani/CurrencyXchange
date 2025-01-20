namespace CurrencyXchange.Models.CustomModels
{
    public class UpdateTransactionsDto
    {
        public int UserId { get; set; }
        public int WalletId { get; set; }
        public string TransactionType { get; set; }
        public long Amount { get; set; }
        public long CurrentBalance { get; set; }
        public int CurrencyId { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
