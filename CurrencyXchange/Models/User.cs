using System;
using System.Collections.Generic;

namespace CurrencyXchange.Models;

public partial class User
{
    public string Name { get; set; } = null!;

    public int CurrencyId { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string Address { get; set; } = null!;

    public int UserId { get; set; }

    public string? Password { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();
}
