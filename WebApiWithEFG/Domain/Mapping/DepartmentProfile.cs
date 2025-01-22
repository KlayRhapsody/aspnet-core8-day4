using System;
using AutoMapper;
using WebApiWithEFG.Data.Entities;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Mapping;

public partial class DepartmentProfile
    : AutoMapper.Profile
{
    public DepartmentProfile()
    {
        CreateMap<WebApiWithEFG.Data.Entities.Department, WebApiWithEFG.Domain.Models.DepartmentReadModel>();

        CreateMap<WebApiWithEFG.Domain.Models.DepartmentCreateModel, WebApiWithEFG.Data.Entities.Department>();

        CreateMap<WebApiWithEFG.Data.Entities.Department, WebApiWithEFG.Domain.Models.DepartmentCreateModel>();

        CreateMap<WebApiWithEFG.Data.Entities.Department, WebApiWithEFG.Domain.Models.DepartmentUpdateModel>();

        CreateMap<WebApiWithEFG.Domain.Models.DepartmentUpdateModel, WebApiWithEFG.Data.Entities.Department>();

        CreateMap<WebApiWithEFG.Domain.Models.DepartmentReadModel, WebApiWithEFG.Domain.Models.DepartmentUpdateModel>();

    }

}
