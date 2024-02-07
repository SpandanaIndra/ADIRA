using System;
using System.Collections.Generic;

namespace ADIRA.Server.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public int? UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int? RoleId { get; set; }

    public int? EntityId { get; set; }

    public int? DepartmentId { get; set; }

    public string? Location { get; set; }

    public virtual DepartmentL? Department { get; set; }

    public virtual EntityL? Entity { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<SecretSantaDatum> SecretSantaDatumEmployees { get; set; } = new List<SecretSantaDatum>();

    public virtual ICollection<SecretSantaDatum> SecretSantaDatumSecretSantaEmployees { get; set; } = new List<SecretSantaDatum>();

    public virtual User? User { get; set; }
}
