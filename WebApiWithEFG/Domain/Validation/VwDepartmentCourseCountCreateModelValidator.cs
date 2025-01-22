using System;
using FluentValidation;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Validation;

public partial class VwDepartmentCourseCountCreateModelValidator
    : AbstractValidator<VwDepartmentCourseCountCreateModel>
{
    public VwDepartmentCourseCountCreateModelValidator()
    {
        #region Generated Constructor
        RuleFor(p => p.Name).MaximumLength(50);
        #endregion
    }

}
