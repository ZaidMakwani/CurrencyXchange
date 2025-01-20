namespace CurrencyXchange.Models.CustomModels
{
    public class MoneyTransferDto
    {
        public int SendersId { get; set; }
        public int ReceiversId { get; set; }
        public int Amount { get; set; }

    }
}
