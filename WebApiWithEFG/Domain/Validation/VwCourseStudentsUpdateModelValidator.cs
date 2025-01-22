using System;
using FluentValidation;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Validation;

public partial class VwCourseStudentsUpdateModelValidator
    : AbstractValidator<VwCourseStudentsUpdateModel>
{
    public VwCourseStudentsUpdateModelValidator()
    {
        #region Generated Constructor
        RuleFor(p => p.DepartmentName).MaximumLength(50);
        RuleFor(p => p.CourseTitle).MaximumLength(50);
        RuleFor(p => p.StudentName).MaximumLength(101);
        #endregion
    }

}
