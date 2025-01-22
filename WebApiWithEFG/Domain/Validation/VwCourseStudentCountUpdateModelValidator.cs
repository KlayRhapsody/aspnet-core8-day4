using System;
using FluentValidation;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Validation;

public partial class VwCourseStudentCountUpdateModelValidator
    : AbstractValidator<VwCourseStudentCountUpdateModel>
{
    public VwCourseStudentCountUpdateModelValidator()
    {
        #region Generated Constructor
        RuleFor(p => p.Name).MaximumLength(50);
        RuleFor(p => p.Title).MaximumLength(50);
        #endregion
    }

}
