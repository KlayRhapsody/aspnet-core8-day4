using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Mapping;

public partial class DepartmentMap
    : IEntityTypeConfiguration<WebApiWithEFG.Data.Entities.Department>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<WebApiWithEFG.Data.Entities.Department> builder)
    {
        #region Generated Configure
        // table
        builder.ToTable("Department", "dbo");

        // key
        builder.HasKey(t => t.DepartmentID);

        // properties
        builder.Property(t => t.DepartmentID)
            .IsRequired()
            .HasColumnName("DepartmentID")
            .HasColumnType("int")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .HasColumnName("Name")
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(t => t.Budget)
            .IsRequired()
            .HasColumnName("Budget")
            .HasColumnType("money");

        builder.Property(t => t.StartDate)
            .IsRequired()
            .HasColumnName("StartDate")
            .HasColumnType("datetime");

        builder.Property(t => t.InstructorID)
            .HasColumnName("InstructorID")
            .HasColumnType("int");

        builder.Property(t => t.RowVersion)
            .IsRequired()
            .HasConversion<byte[]>()
            .IsRowVersion()
            .IsConcurrencyToken()
            .HasColumnName("RowVersion")
            .HasColumnType("rowversion")
            .ValueGeneratedOnAddOrUpdate();

        // relationships
        builder.HasOne(t => t.InstructorPerson)
            .WithMany(t => t.InstructorDepartments)
            .HasForeignKey(d => d.InstructorID)
            .HasConstraintName("FK_dbo.Department_dbo.Instructor_InstructorID");

        #endregion
    }

    #region Generated Constants
    public readonly struct Table
    {
        public const string Schema = "dbo";
        public const string Name = "Department";
    }

    public readonly struct Columns
    {
        public const string DepartmentID = "DepartmentID";
        public const string Name = "Name";
        public const string Budget = "Budget";
        public const string StartDate = "StartDate";
        public const string InstructorID = "InstructorID";
        public const string RowVersion = "RowVersion";
    }
    #endregion
}
