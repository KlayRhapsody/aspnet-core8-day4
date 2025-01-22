using System;
using FluentValidation;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Validation;

public partial class CourseCreateModelValidator
    : AbstractValidator<CourseCreateModel>
{
    public CourseCreateModelValidator()
    {
        #region Generated Constructor
        RuleFor(p => p.Title).MaximumLength(50);
        #endregion
    }

}
