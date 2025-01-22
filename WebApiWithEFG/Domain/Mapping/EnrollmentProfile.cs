using System;
using AutoMapper;
using WebApiWithEFG.Data.Entities;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Mapping;

public partial class EnrollmentProfile
    : AutoMapper.Profile
{
    public EnrollmentProfile()
    {
        CreateMap<WebApiWithEFG.Data.Entities.Enrollment, WebApiWithEFG.Domain.Models.EnrollmentReadModel>();

        CreateMap<WebApiWithEFG.Domain.Models.EnrollmentCreateModel, WebApiWithEFG.Data.Entities.Enrollment>();

        CreateMap<WebApiWithEFG.Data.Entities.Enrollment, WebApiWithEFG.Domain.Models.EnrollmentCreateModel>();

        CreateMap<WebApiWithEFG.Data.Entities.Enrollment, WebApiWithEFG.Domain.Models.EnrollmentUpdateModel>();

        CreateMap<WebApiWithEFG.Domain.Models.EnrollmentUpdateModel, WebApiWithEFG.Data.Entities.Enrollment>();

        CreateMap<WebApiWithEFG.Domain.Models.EnrollmentReadModel, WebApiWithEFG.Domain.Models.EnrollmentUpdateModel>();

    }

}
