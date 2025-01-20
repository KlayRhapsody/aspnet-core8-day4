# ASP.NET Core 8 開發實戰：資料存取篇 (EF Core)

### **使用 EF Power Tools 進行資料庫逆向工程注意事項**

詳細逆向工程操作步驟請參閱 [在 Minimal API 中使用 Entity Framework](https://github.com/KlayRhapsody/aspnet-core8-day2?tab=readme-ov-file#%E5%9C%A8-minimal-api-%E4%B8%AD%E4%BD%BF%E7%94%A8-entity-framework)。

連線字串在使用時需加上 `;TrustServerCertificate=True` 允許客戶端信任伺服器的憑證，否則會導致 SSL 驗證失敗。

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ContosoUniversity;User Id=XX;Password=XXXXXXX;Encrypt=true;TrustServerCertificate=True"
  }
}
```

`efcpt-config.json` 將 `refresh-object-lists` 設定為 `false`，可以避免在進行反向工程時重新整理物件清單，從而防止這些設定被覆蓋掉。

```json
{
  "code-generation": {
    "refresh-object-lists": false,
  }
}
```


### **方法語法（Method Syntax）或流式語法（Fluent Syntax）**

```csharp
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

    return Ok(data);
}
```

對應的 SQL 查詢語法

```SQL
Executed DbCommand (36ms) [Parameters=[@__query_0_contains='?' (Size = 50)], CommandType='Text', CommandTimeout='30']
      SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
      FROM [Course] AS [c]
      INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
      WHERE [c].[Title] LIKE @__query_0_contains ESCAPE N'\'
```


### **查詢語法（Query Syntax）**

```csharp
public IActionResult Get(string query)
{
    var data = from item in _context.Courses
            join d in _context.Departments on item.DepartmentId equals d.DepartmentId
            where item.Title!.Contains(query)
            select new
            {
                item.CourseId,
                item.Title,
                item.Credits,
                DepartmentName = d.Name
            };

    return Ok(data);
}
```


### **請用 Log 輸出查詢參數**

```csharp
builder.Services.AddDbContext<ContosoUniversityContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging();
});
```

```sql
Executed DbCommand (16ms) [Parameters=[@__query_0_contains='%asp%' (Size = 50)], CommandType='Text', CommandTimeout='30']
      SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
      FROM [Course] AS [c]
      INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
      WHERE [c].[Title] LIKE @__query_0_contains ESCAPE N'\'
```


### **使用 EF Core 不會發生 SQL Injection**

已經參數化的查詢語句，即使使用者輸入了 SQL Injection 的內容，也不會對資料庫造成影響。

```bash
GET {{WebApi_HostAddress}}/weatherforecast/?query=Git;%20DROP%20TABLE%20Course
Accept: application/json
```

```sql
Executed DbCommand (95ms) [Parameters=[@__query_0_contains='%Git; DROP TABLE Course%' (Size = 50)], CommandType='Text', CommandTimeout='30']
      SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
      FROM [Course] AS [c]
      INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
      WHERE CONVERT(date, [d].[StartDate]) = '2015-02-01T00:00:00.000' AND [c].[Title] LIKE @__query_0_contains ESCAPE N'\'
```


### **使用 view 進行查詢邏輯封裝**

先於資料庫中建立一個 view

```sql
CREATE VIEW vwMyDepartCourse AS
SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
      FROM [Course] AS [c]
      INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
```

使用逆向工程將 view 加入到 DbContext 中，並調整對應程式碼使用方式

```csharp
public IActionResult GetMyDepartCourses(string? query)
{
    // var data = _context.VwMyDepartCourses.AsQueryable();
    var data = from item in _context.VwMyDepartCourses
            select item;

    if (!string.IsNullOrEmpty(query))
        data = data.Where(item => item.Title!.Contains(query));

    return Ok(data);
}
```


### **使用 Store Procedure 進行查詢邏輯封裝**

先於資料庫中建立一個 Store Procedure

```sql
CREATE PROCEDURE GetMyDepartCourses
    @query NVARCHAR(MAX) = NULL
AS
BEGIN
    SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
    FROM [Course] AS [c]
    INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
    WHERE (@query IS NULL OR [c].[Title] LIKE '%' + @query + '%')
END
```

使用逆向工程將 Store Procedure 加入到 DbContext 中，並調整對應程式碼使用方式

```csharp
public async Task<IActionResult> GetMyDepartCoursesSP(string? query)
{
    var data = await _context.GetProcedures().GetMyDepartCoursesAsync(query);

    return Ok(data);
}
```


