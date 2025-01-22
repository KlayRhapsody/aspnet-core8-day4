using System;
using AutoMapper;
using WebApiWithEFG.Data.Entities;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Mapping;

public partial class VwDepartmentCourseCountProfile
    : AutoMapper.Profile
{
    public VwDepartmentCourseCountProfile()
    {
        CreateMap<WebApiWithEFG.Data.Entities.VwDepartmentCourseCount, WebApiWithEFG.Domain.Models.VwDepartmentCourseCountReadModel>();

        CreateMap<WebApiWithEFG.Domain.Models.VwDepartmentCourseCountCreateModel, WebApiWithEFG.Data.Entities.VwDepartmentCourseCount>();

        CreateMap<WebApiWithEFG.Data.Entities.VwDepartmentCourseCount, WebApiWithEFG.Domain.Models.VwDepartmentCourseCountCreateModel>();

        CreateMap<WebApiWithEFG.Data.Entities.VwDepartmentCourseCount, WebApiWithEFG.Domain.Models.VwDepartmentCourseCountUpdateModel>();

        CreateMap<WebApiWithEFG.Domain.Models.VwDepartmentCourseCountUpdateModel, WebApiWithEFG.Data.Entities.VwDepartmentCourseCount>();

        CreateMap<WebApiWithEFG.Domain.Models.VwDepartmentCourseCountReadModel, WebApiWithEFG.Domain.Models.VwDepartmentCourseCountUpdateModel>();

    }

}
