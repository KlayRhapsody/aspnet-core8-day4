using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Mapping;

public partial class VwCourseStudentsMap
    : IEntityTypeConfiguration<WebApiWithEFG.Data.Entities.VwCourseStudents>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<WebApiWithEFG.Data.Entities.VwCourseStudents> builder)
    {
        #region Generated Configure
        // table
        builder.ToView("vwCourseStudents", "dbo");

        // key
        builder.HasNoKey();

        // properties
        builder.Property(t => t.DepartmentID)
            .HasColumnName("DepartmentID")
            .HasColumnType("int");

        builder.Property(t => t.DepartmentName)
            .HasColumnName("DepartmentName")
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(t => t.CourseID)
            .IsRequired()
            .HasColumnName("CourseID")
            .HasColumnType("int");

        builder.Property(t => t.CourseTitle)
            .HasColumnName("CourseTitle")
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(t => t.StudentID)
            .HasColumnName("StudentID")
            .HasColumnType("int");

        builder.Property(t => t.StudentName)
            .HasColumnName("StudentName")
            .HasColumnType("nvarchar(101)")
            .HasMaxLength(101);

        // relationships
        #endregion
    }

    #region Generated Constants
    public readonly struct Table
    {
        public const string Schema = "dbo";
        public const string Name = "vwCourseStudents";
    }

    public readonly struct Columns
    {
        public const string DepartmentID = "DepartmentID";
        public const string DepartmentName = "DepartmentName";
        public const string CourseID = "CourseID";
        public const string CourseTitle = "CourseTitle";
        public const string StudentID = "StudentID";
        public const string StudentName = "StudentName";
    }
    #endregion
}
