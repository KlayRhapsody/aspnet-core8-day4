using System;
using System.Collections.Generic;

namespace WebApiWithEFG.Domain.Models;

public partial class CourseCreateModel
{
    #region Generated Properties
    public int CourseID { get; set; }

    public string? Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentID { get; set; }

    #endregion

}
