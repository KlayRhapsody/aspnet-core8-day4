using System;
using AutoMapper;
using WebApiWithEFG.Data.Entities;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Mapping;

public partial class CourseProfile
    : AutoMapper.Profile
{
    public CourseProfile()
    {
        CreateMap<WebApiWithEFG.Data.Entities.Course, WebApiWithEFG.Domain.Models.CourseReadModel>();

        CreateMap<WebApiWithEFG.Domain.Models.CourseCreateModel, WebApiWithEFG.Data.Entities.Course>();

        CreateMap<WebApiWithEFG.Data.Entities.Course, WebApiWithEFG.Domain.Models.CourseCreateModel>();

        CreateMap<WebApiWithEFG.Data.Entities.Course, WebApiWithEFG.Domain.Models.CourseUpdateModel>();

        CreateMap<WebApiWithEFG.Domain.Models.CourseUpdateModel, WebApiWithEFG.Data.Entities.Course>();

        CreateMap<WebApiWithEFG.Domain.Models.CourseReadModel, WebApiWithEFG.Domain.Models.CourseUpdateModel>();

    }

}
