using System;
using System.Collections.Generic;

namespace CurrencyXchange.Models;

public partial class UserTransaction
{
    public int WalletId { get; set; }

    public int UserId { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal? Amount { get; set; }

    public decimal? Balance { get; set; }

    public DateTime Time { get; set; }

    public int CurrencyId { get; set; }

    public int Id { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
