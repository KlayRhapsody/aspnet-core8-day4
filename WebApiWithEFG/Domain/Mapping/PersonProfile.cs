using System;
using AutoMapper;
using WebApiWithEFG.Data.Entities;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Domain.Mapping;

public partial class PersonProfile
    : AutoMapper.Profile
{
    public PersonProfile()
    {
        CreateMap<WebApiWithEFG.Data.Entities.Person, WebApiWithEFG.Domain.Models.PersonReadModel>();

        CreateMap<WebApiWithEFG.Domain.Models.PersonCreateModel, WebApiWithEFG.Data.Entities.Person>();

        CreateMap<WebApiWithEFG.Data.Entities.Person, WebApiWithEFG.Domain.Models.PersonCreateModel>();

        CreateMap<WebApiWithEFG.Data.Entities.Person, WebApiWithEFG.Domain.Models.PersonUpdateModel>();

        CreateMap<WebApiWithEFG.Domain.Models.PersonUpdateModel, WebApiWithEFG.Data.Entities.Person>();

        CreateMap<WebApiWithEFG.Domain.Models.PersonReadModel, WebApiWithEFG.Domain.Models.PersonUpdateModel>();

    }

}
