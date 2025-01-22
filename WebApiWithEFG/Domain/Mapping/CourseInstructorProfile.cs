using System;
using AutoMapper;
using WebApiWithEFG.Data.Entities;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Mapping;

public partial class CourseInstructorProfile
    : AutoMapper.Profile
{
    public CourseInstructorProfile()
    {
        CreateMap<WebApiWithEFG.Data.Entities.CourseInstructor, WebApiWithEFG.Domain.Models.CourseInstructorReadModel>();

        CreateMap<WebApiWithEFG.Domain.Models.CourseInstructorCreateModel, WebApiWithEFG.Data.Entities.CourseInstructor>();

        CreateMap<WebApiWithEFG.Data.Entities.CourseInstructor, WebApiWithEFG.Domain.Models.CourseInstructorCreateModel>();

        CreateMap<WebApiWithEFG.Data.Entities.CourseInstructor, WebApiWithEFG.Domain.Models.CourseInstructorUpdateModel>();

        CreateMap<WebApiWithEFG.Domain.Models.CourseInstructorUpdateModel, WebApiWithEFG.Data.Entities.CourseInstructor>();

        CreateMap<WebApiWithEFG.Domain.Models.CourseInstructorReadModel, WebApiWithEFG.Domain.Models.CourseInstructorUpdateModel>();

    }

}
