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