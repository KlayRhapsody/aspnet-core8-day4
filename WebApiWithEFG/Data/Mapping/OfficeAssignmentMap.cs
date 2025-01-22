using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Mapping;

public partial class OfficeAssignmentMap
    : IEntityTypeConfiguration<WebApiWithEFG.Data.Entities.OfficeAssignment>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<WebApiWithEFG.Data.Entities.OfficeAssignment> builder)
    {
        #region Generated Configure
        // table
        builder.ToTable("OfficeAssignment", "dbo");

        // key
        builder.HasKey(t => t.InstructorID);

        // properties
        builder.Property(t => t.InstructorID)
            .IsRequired()
            .HasColumnName("InstructorID")
            .HasColumnType("int");

        builder.Property(t => t.Location)
            .HasColumnName("Location")
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        // relationships
        builder.HasOne(t => t.InstructorPerson)
            .WithOne(t => t.InstructorOfficeAssignment)
            .HasForeignKey<WebApiWithEFG.Data.Entities.OfficeAssignment>(d => d.InstructorID)
            .HasConstraintName("FK_dbo.OfficeAssignment_dbo.Instructor_InstructorID");

        #endregion
    }

    #region Generated Constants
    public readonly struct Table
    {
        public const string Schema = "dbo";
        public const string Name = "OfficeAssignment";
    }

    public readonly struct Columns
    {
        public const string InstructorID = "InstructorID";
        public const string Location = "Location";
    }
    #endregion
}
