using System;
using FluentValidation;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Validation;

public partial class EnrollmentCreateModelValidator
    : AbstractValidator<EnrollmentCreateModel>
{
    public EnrollmentCreateModelValidator()
    {
        #region Generated Constructor
        #endregion
    }

}
