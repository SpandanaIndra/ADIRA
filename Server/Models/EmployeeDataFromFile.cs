using System;
using System.Collections.Generic;

namespace ADIRA.Server.Models;

public partial class EmployeeDataFromFile
{
    public int EmployeeDataFromFileId { get; set; }

    public string? Id { get; set; }

    public string? Entity { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Department { get; set; }
}
