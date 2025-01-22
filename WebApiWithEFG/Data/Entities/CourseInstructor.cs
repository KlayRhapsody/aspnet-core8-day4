using System;
using System.Collections.Generic;

namespace WebApiWithEFG.Data.Entities;

public partial class CourseInstructor
{
    public CourseInstructor()
    {
        #region Generated Constructor
        #endregion
    }

    #region Generated Properties
    public int CourseID { get; set; }

    public int InstructorID { get; set; }

    #endregion

    #region Generated Relationships
    public virtual Course Course { get; set; } = null!;

    public virtual Person InstructorPerson { get; set; } = null!;

    #endregion

}
