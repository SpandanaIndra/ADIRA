using System;
using System.Collections.Generic;

namespace ADIRA.Server.Models;

public partial class SecretSantaDatum
{
    public int Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string SecretSantaEmployeeId { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool EmailSent { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employee SecretSantaEmployee { get; set; } = null!;
}
