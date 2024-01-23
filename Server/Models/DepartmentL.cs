﻿using System;
using System.Collections.Generic;

namespace ADIRA.Server.Models;

public partial class DepartmentL
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? InActiveDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
