using System;
using AutoMapper;
using WebApiWithEFG.Data.Entities;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Mapping;

public partial class VwCourseStudentsProfile
    : AutoMapper.Profile
{
    public VwCourseStudentsProfile()
    {
        CreateMap<WebApiWithEFG.Data.Entities.VwCourseStudents, WebApiWithEFG.Domain.Models.VwCourseStudentsReadModel>();

        CreateMap<WebApiWithEFG.Domain.Models.VwCourseStudentsCreateModel, WebApiWithEFG.Data.Entities.VwCourseStudents>();

        CreateMap<WebApiWithEFG.Data.Entities.VwCourseStudents, WebApiWithEFG.Domain.Models.VwCourseStudentsCreateModel>();

        CreateMap<WebApiWithEFG.Data.Entities.VwCourseStudents, WebApiWithEFG.Domain.Models.VwCourseStudentsUpdateModel>();

        CreateMap<WebApiWithEFG.Domain.Models.VwCourseStudentsUpdateModel, WebApiWithEFG.Data.Entities.VwCourseStudents>();

        CreateMap<WebApiWithEFG.Domain.Models.VwCourseStudentsReadModel, WebApiWithEFG.Domain.Models.VwCourseStudentsUpdateModel>();

    }

}
