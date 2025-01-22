using System;
using FluentValidation;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Validation;

public partial class VwDepartmentCourseCountUpdateModelValidator
    : AbstractValidator<VwDepartmentCourseCountUpdateModel>
{
    public VwDepartmentCourseCountUpdateModelValidator()
    {
        #region Generated Constructor
        RuleFor(p => p.Name).MaximumLength(50);
        #endregion
    }

}
