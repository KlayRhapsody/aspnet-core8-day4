using System;
using AutoMapper;
using WebApiWithEFG.Data.Entities;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Mapping;

public partial class VwCourseStudentCountProfile
    : AutoMapper.Profile
{
    public VwCourseStudentCountProfile()
    {
        CreateMap<WebApiWithEFG.Data.Entities.VwCourseStudentCount, WebApiWithEFG.Domain.Models.VwCourseStudentCountReadModel>();

        CreateMap<WebApiWithEFG.Domain.Models.VwCourseStudentCountCreateModel, WebApiWithEFG.Data.Entities.VwCourseStudentCount>();

        CreateMap<WebApiWithEFG.Data.Entities.VwCourseStudentCount, WebApiWithEFG.Domain.Models.VwCourseStudentCountCreateModel>();

        CreateMap<WebApiWithEFG.Data.Entities.VwCourseStudentCount, WebApiWithEFG.Domain.Models.VwCourseStudentCountUpdateModel>();

        CreateMap<WebApiWithEFG.Domain.Models.VwCourseStudentCountUpdateModel, WebApiWithEFG.Data.Entities.VwCourseStudentCount>();

        CreateMap<WebApiWithEFG.Domain.Models.VwCourseStudentCountReadModel, WebApiWithEFG.Domain.Models.VwCourseStudentCountUpdateModel>();

    }

}
