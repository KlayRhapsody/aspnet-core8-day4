using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiWithEFG.Data;
using WebApiWithEFG.Domain.Models;

namespace WebApiWithEFG.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ContosoUniversityEFGContext _context;
    private readonly IMapper _mapper;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, 
        ContosoUniversityEFGContext context,
        IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<CourseReadModel> Get()
    {
        // var data = _context.Courses.Select(c => new CourseReadModel
        // {
        //     CourseID = c.CourseID,
        //     Title = c.Title,
        //     Credits = c.Credits,
        //     DepartmentID = c.DepartmentID
        // });

        var data = _context.Courses.ProjectTo<CourseReadModel>(_mapper.ConfigurationProvider);

        return data;
    }
}
