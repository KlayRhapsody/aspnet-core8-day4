using System;
using System.Collections.Generic;

namespace WebApiWithEFG.Domain.Models;

public partial class DepartmentReadModel
{
    #region Generated Properties
    public int DepartmentID { get; set; }

    public string? Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public int? InstructorID { get; set; }

    public long RowVersion { get; set; }

    #endregion

}
