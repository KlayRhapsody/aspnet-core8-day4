using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Mapping;

public partial class VwDepartmentCourseCountMap
    : IEntityTypeConfiguration<WebApiWithEFG.Data.Entities.VwDepartmentCourseCount>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<WebApiWithEFG.Data.Entities.VwDepartmentCourseCount> builder)
    {
        #region Generated Configure
        // table
        builder.ToView("vwDepartmentCourseCount", "dbo");

        // key
        builder.HasNoKey();

        // properties
        builder.Property(t => t.DepartmentID)
            .IsRequired()
            .HasColumnName("DepartmentID")
            .HasColumnType("int");

        builder.Property(t => t.Name)
            .HasColumnName("Name")
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(t => t.CourseCount)
            .HasColumnName("CourseCount")
            .HasColumnType("int");

        // relationships
        #endregion
    }

    #region Generated Constants
    public readonly struct Table
    {
        public const string Schema = "dbo";
        public const string Name = "vwDepartmentCourseCount";
    }

    public readonly struct Columns
    {
        public const string DepartmentID = "DepartmentID";
        public const string Name = "Name";
        public const string CourseCount = "CourseCount";
    }
    #endregion
}
