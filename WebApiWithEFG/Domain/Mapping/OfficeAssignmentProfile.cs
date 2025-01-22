using System;
using AutoMapper;
using WebApiWithEFG.Data.Entities;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Mapping;

public partial class OfficeAssignmentProfile
    : AutoMapper.Profile
{
    public OfficeAssignmentProfile()
    {
        CreateMap<WebApiWithEFG.Data.Entities.OfficeAssignment, WebApiWithEFG.Domain.Models.OfficeAssignmentReadModel>();

        CreateMap<WebApiWithEFG.Domain.Models.OfficeAssignmentCreateModel, WebApiWithEFG.Data.Entities.OfficeAssignment>();

        CreateMap<WebApiWithEFG.Data.Entities.OfficeAssignment, WebApiWithEFG.Domain.Models.OfficeAssignmentCreateModel>();

        CreateMap<WebApiWithEFG.Data.Entities.OfficeAssignment, WebApiWithEFG.Domain.Models.OfficeAssignmentUpdateModel>();

        CreateMap<WebApiWithEFG.Domain.Models.OfficeAssignmentUpdateModel, WebApiWithEFG.Data.Entities.OfficeAssignment>();

        CreateMap<WebApiWithEFG.Domain.Models.OfficeAssignmentReadModel, WebApiWithEFG.Domain.Models.OfficeAssignmentUpdateModel>();

    }

}
