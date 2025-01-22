using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApiWithEFG.Data;

public partial class ContosoUniversityEFGContext : DbContext
{
    public ContosoUniversityEFGContext(DbContextOptions<ContosoUniversityEFGContext> options)
        : base(options)
    {
    }

    #region Generated Properties
    public virtual DbSet<WebApiWithEFG.Data.Entities.CourseInstructor> CourseInstructors { get; set; } = null!;

    public virtual DbSet<WebApiWithEFG.Data.Entities.Course> Courses { get; set; } = null!;

    public virtual DbSet<WebApiWithEFG.Data.Entities.Department> Departments { get; set; } = null!;

    public virtual DbSet<WebApiWithEFG.Data.Entities.Enrollment> Enrollments { get; set; } = null!;

    public virtual DbSet<WebApiWithEFG.Data.Entities.OfficeAssignment> OfficeAssignments { get; set; } = null!;

    public virtual DbSet<WebApiWithEFG.Data.Entities.Person> People { get; set; } = null!;

    public virtual DbSet<WebApiWithEFG.Data.Entities.VwCourseStudentCount> VwCourseStudentCounts { get; set; } = null!;

    public virtual DbSet<WebApiWithEFG.Data.Entities.VwCourseStudents> VwCourseStudents { get; set; } = null!;

    public virtual DbSet<WebApiWithEFG.Data.Entities.VwDepartmentCourseCount> VwDepartmentCourseCounts { get; set; } = null!;

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Generated Configuration
        modelBuilder.ApplyConfiguration(new WebApiWithEFG.Data.Mapping.CourseInstructorMap());
        modelBuilder.ApplyConfiguration(new WebApiWithEFG.Data.Mapping.CourseMap());
        modelBuilder.ApplyConfiguration(new WebApiWithEFG.Data.Mapping.DepartmentMap());
        modelBuilder.ApplyConfiguration(new WebApiWithEFG.Data.Mapping.EnrollmentMap());
        modelBuilder.ApplyConfiguration(new WebApiWithEFG.Data.Mapping.OfficeAssignmentMap());
        modelBuilder.ApplyConfiguration(new WebApiWithEFG.Data.Mapping.PersonMap());
        modelBuilder.ApplyConfiguration(new WebApiWithEFG.Data.Mapping.VwCourseStudentCountMap());
        modelBuilder.ApplyConfiguration(new WebApiWithEFG.Data.Mapping.VwCourseStudentsMap());
        modelBuilder.ApplyConfiguration(new WebApiWithEFG.Data.Mapping.VwDepartmentCourseCountMap());
        #endregion
    }
}
