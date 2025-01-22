using System;
using FluentValidation;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Validation;

public partial class EnrollmentUpdateModelValidator
    : AbstractValidator<EnrollmentUpdateModel>
{
    public EnrollmentUpdateModelValidator()
    {
        #region Generated Constructor
        #endregion
    }

}
