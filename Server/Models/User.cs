using System;
using System.Collections.Generic;

namespace ADIRA.Server.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime LastLoginDate { get; set; }

    public int? LoginAttempts { get; set; }

    public bool? IsBlocked { get; set; }

    public DateTime? BlockExpirationDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
