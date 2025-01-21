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


### **使用 Raw SQL 進行查詢**

1. 使用 `FromSqlRaw` 方法（可能有 SQL Injection 風險）
   - 如果直接將參數拼接到 SQL 查詢語句（如 `sqlQuery` 的第一種寫法），會有 SQL Injection 的風險。
   - 不建議直接將用戶輸入拼接到查詢語句中，應避免這種用法。

2. 使用 `FromSql` 方法（安全）
   - 其行為等同於 `FromSqlInterpolated`，可自動進行參數化處理，避免 SQL Injection 風險。

3. 使用 `FromSqlInterpolated` 方法（安全）
   - `FromSqlInterpolated` 的參數為 `FormattableString` 類型。
   - 它會自動將內插變數轉換為參數化查詢，確保安全性，並防止 SQL Injection。
   - 推薦使用這種方式來處理帶參數的查詢語句，特別是在需要直接內插變數的情境下。

4. 使用 `FromSqlRaw` 方法並帶入參數（安全）
   - 若使用 `FromSqlRaw`，需要明確提供參數化的查詢。
   - 可以通過 `SqlParameter` 或匿名物件傳遞參數，這樣也可以避免 SQL Injection 的風險。
     - 使用自定義名稱的參數（如 `@query`）。
     - 或使用自動命名的參數（如 `@p0`、`@p1` 等），EF Core 會自動處理多個參數的對應。


```csharp
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
```


### **循環參考問題（Loop Reference Issue）**

在查詢語句中使用 `Include` 方法時，可能會導致循環參考問題，例如 `Department` 類別中包含了 `Course` 類別，而 `Course` 類別中又包含了 `Department` 類別。

```bash
:System.Text.Json.JsonException: A possible object cycle was detected. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
```

解決方法一是使用 `JsonIgnore` 屬性來忽略循環參考，不適用於 DB First 的開發情境，因為會調整到 EF Core 產生的程式碼。

```csharp
[JsonIgnore]
public virtual ICollection<Course> Courses { get; set; }
```

解決方法二是使用 DTO 來避免循環參考，自定義一個 DTO 類別，只包含需要的屬性。

```csharp
public class InstructorsResponse
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Discriminator { get; set; } = null!;
}
```

先 `Inclue` 關聯表，再使用 `Select` 或 `SelectMany` 方法將需要的屬性映射到 DTO 類別中。

這裡選用 `SelectMany` 方法，將 `Courses` 表和 `Instructors` 表進行聯集操作，並將結果映射到 `InstructorsResponse` 類別中。

`SelectMany` 是一個 LINQ 擴充方法，用來展開集合中的集合，將多個集合中的元素合併到一個集合中。

```csharp
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
```


### **使用 EF Core Power Tools 產生 Diagram**

調整設定後再執行 `efcpt ...` 指令，即可生成 Mermaid Diagram。

```json
{
    "generate-mermaid-diagram": true,
}
```

