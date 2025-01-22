using System;
using FluentValidation;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Validation;

public partial class CourseInstructorUpdateModelValidator
    : AbstractValidator<CourseInstructorUpdateModel>
{
    public CourseInstructorUpdateModelValidator()
    {
        #region Generated Constructor
        #endregion
    }

}
