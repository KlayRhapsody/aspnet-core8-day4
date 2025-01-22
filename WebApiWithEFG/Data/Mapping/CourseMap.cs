using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Mapping;

public partial class CourseMap
    : IEntityTypeConfiguration<WebApiWithEFG.Data.Entities.Course>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<WebApiWithEFG.Data.Entities.Course> builder)
    {
        #region Generated Configure
        // table
        builder.ToTable("Course", "dbo");

        // key
        builder.HasKey(t => t.CourseID);

        // properties
        builder.Property(t => t.CourseID)
            .IsRequired()
            .HasColumnName("CourseID")
            .HasColumnType("int")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .HasColumnName("Title")
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(t => t.Credits)
            .IsRequired()
            .HasColumnName("Credits")
            .HasColumnType("int");

        builder.Property(t => t.DepartmentID)
            .IsRequired()
            .HasColumnName("DepartmentID")
            .HasColumnType("int")
            .HasDefaultValue(1);

        // relationships
        builder.HasOne(t => t.Department)
            .WithMany(t => t.Courses)
            .HasForeignKey(d => d.DepartmentID)
            .HasConstraintName("FK_dbo.Course_dbo.Department_DepartmentID");

        #endregion
    }

    #region Generated Constants
    public readonly struct Table
    {
        public const string Schema = "dbo";
        public const string Name = "Course";
    }

    public readonly struct Columns
    {
        public const string CourseID = "CourseID";
        public const string Title = "Title";
        public const string Credits = "Credits";
        public const string DepartmentID = "DepartmentID";
    }
    #endregion
}
