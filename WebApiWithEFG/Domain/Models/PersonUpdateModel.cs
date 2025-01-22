using System;
using System.Collections.Generic;

namespace WebApiWithEFG.Domain.Models;

public partial class PersonUpdateModel
{
    #region Generated Properties
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public DateTime? HireDate { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public string Discriminator { get; set; } = null!;

    #endregion

}
