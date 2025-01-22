using System;
using FluentValidation;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Validation;

public partial class CourseInstructorCreateModelValidator
    : AbstractValidator<CourseInstructorCreateModel>
{
    public CourseInstructorCreateModelValidator()
    {
        #region Generated Constructor
        #endregion
    }

}
