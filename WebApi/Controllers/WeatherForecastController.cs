namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ContosoUniversityContext _context;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        ContosoUniversityContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get(string query)
    {
        var data = _context.Courses
            .Include(item => item.Department)
            .Where(item => item.Title!.Contains(query))
            .Select(item => new
            {
                item.CourseId,
                item.Title,
                item.Credits,
                DepartmentName = item.Department!.Name
            })
            .ToList();

        // var data = from item in _context.Courses
        //         join d in _context.Departments on item.DepartmentId equals d.DepartmentId
        //         where item.Title!.Contains(query)
        //         select new
        //         {
        //             item.CourseId,
        //             item.Title,
        //             item.Credits,
        //             DepartmentName = d.Name
        //         };

        return Ok(data);
    }
}
