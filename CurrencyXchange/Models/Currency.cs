using System;
using System.Collections.Generic;

namespace CurrencyXchange.Models;

public partial class Currency
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public int Id { get; set; }

    public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
