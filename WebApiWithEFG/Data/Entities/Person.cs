using System;
using System.Collections.Generic;

namespace WebApiWithEFG.Data.Entities;

public partial class Person
{
    public Person()
    {
        #region Generated Constructor
        InstructorCourseInstructors = new HashSet<CourseInstructor>();
        InstructorDepartments = new HashSet<Department>();
        StudentEnrollments = new HashSet<Enrollment>();
        #endregion
    }

    #region Generated Properties
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public DateTime? HireDate { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public string Discriminator { get; set; } = null!;

    #endregion

    #region Generated Relationships
    public virtual ICollection<CourseInstructor> InstructorCourseInstructors { get; set; }

    public virtual ICollection<Department> InstructorDepartments { get; set; }

    public virtual OfficeAssignment InstructorOfficeAssignment { get; set; } = null!;

    public virtual ICollection<Enrollment> StudentEnrollments { get; set; }

    #endregion

}
