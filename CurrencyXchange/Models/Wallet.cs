using System;
using System.Collections.Generic;

namespace CurrencyXchange.Models;

public partial class Wallet
{
    public int UserId { get; set; }

    public decimal? Balance { get; set; }

    public int Id { get; set; }

    public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();
}
