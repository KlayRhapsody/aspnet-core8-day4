
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(options =>
{
    var httpContextAccessor = options.GetRequiredService<IHttpContextAccessor>();
    httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("X-Database-Name", out var dbName);
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
    conn = conn!.Replace("DB", dbName);
    var dbContextOptionsBuilder = new DbContextOptionsBuilder<ContosoUniversityContext>().UseSqlServer(conn);
    return new ContosoUniversityContext(dbContextOptionsBuilder.Options);
});

// builder.Services.AddDbContext<ContosoUniversityContext>(
//     options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
