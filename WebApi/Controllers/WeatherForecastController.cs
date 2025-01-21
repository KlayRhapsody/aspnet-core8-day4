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
    public IActionResult Get(string? query)
    {
        // var data = _context.Courses
        //     .Include(item => item.Department)
        //     .Where(item => item.Title!.Contains(query))
        //     .Select(item => new
        //     {
        //         item.CourseId,
        //         item.Title,
        //         item.Credits,
        //         DepartmentName = item.Department!.Name
        //     })
        //     .ToList();

        var data = from item in _context.Courses
                join d in _context.Departments on item.DepartmentId equals d.DepartmentId
                // where d.StartDate.Date == DateTime.Parse("2015-03-21")
                select new
                {
                    item.CourseId,
                    item.Title,
                    item.Credits,
                    DepartmentName = d.Name
                };

        if (!string.IsNullOrEmpty(query))
            data = data.Where(item => item.Title.Contains(query));

        return Ok(data);
    }

    // GET: WeatherForecast/myDepartCourses
    [HttpGet("MyDepartCourses", Name = "GetMyDepartCourses")]
    public IActionResult GetMyDepartCourses(string? query)
    {
        // var data = _context.VwMyDepartCourses.AsQueryable();
        var data = from item in _context.VwMyDepartCourses
                select item;

        if (!string.IsNullOrEmpty(query))
            data = data.Where(item => item.Title!.Contains(query));

        return Ok(data);
    }

    [HttpGet("MyDepartCoursesSP", Name = "GetMyDepartCoursesSP")]
    public async Task<IActionResult> GetMyDepartCoursesSP(string? query)
    {
        var data = await _context.GetProcedures().GetMyDepartCoursesAsync(query);

        return Ok(data);
    }
    
    [HttpGet("MyDepartCoursesSQL", Name = "GetMyDepartCoursesSQL")]
    public async Task<IActionResult> GetMyDepartCoursesSQL(string? query)
    {
        var sqlQuery = $"""
            SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
            FROM [Course] AS [c]
            INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
            WHERE [c].[Title] LIKE '%{query}%'
            """;

        var data1 = await _context.VwMyDepartCourses.FromSqlRaw(sqlQuery).ToListAsync();

        var data2 = await _context.VwMyDepartCourses.FromSql(
            $"""
            SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
            FROM [Course] AS [c]
            INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
            WHERE [c].[Title] LIKE '%' + {query} + '%'
            """).ToListAsync();

        var data3 = _context.VwMyDepartCourses.FromSqlInterpolated(
            $"""
            SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
            FROM [Course] AS [c]
            INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
            WHERE [c].[Title] LIKE '%' + {query} + '%'
            """);

        var data4 = _context.VwMyDepartCourses.FromSqlRaw(
            $"""
            SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
            FROM [Course] AS [c]
            INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
            WHERE [c].[Title] LIKE '%' + @query + '%'
            """, new SqlParameter("@query", query)); // 或是直接使用 @p0

        return Ok(data4);
    }

    [HttpGet("CourseInstructors", Name = "GetCourseInstructors")]
    public async Task<ActionResult<InstructorsResponse>> GetCourseInstructors()
    {
        var data = await _context.Courses
            .Include(item => item.Instructors)
            .SelectMany(c => c.Instructors, (c, i) => new InstructorsResponse
            {
                Id = i.Id,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Discriminator = i.Discriminator
            })
            .ToListAsync();

        return Ok(data);
    }
        
}
