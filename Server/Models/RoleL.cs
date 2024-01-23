using System;
using System.Collections.Generic;

namespace ADIRA.Server.Models;

public partial class RoleL
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsActive { get; set; }

    public string Code { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
