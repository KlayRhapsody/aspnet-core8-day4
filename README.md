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


### **使用 EntityFrameworkCore.Generator 進行程式碼生成**

這是一套會用到 AutoMapper、FluentValidation 等套件的程式碼生成工具。
- `AutoMapper`：用於對象映射。
- `FluentValidation`：用於模型驗證。

```bash
# 安裝 EntityFrameworkCore.Generator 工具
dotnet tool install --global EntityFrameworkCore.Generator

# 新增一個 Web API 專案
dotnet new webapi -n WebApiWithEFG

# 先自建 scret id 也可以
dotnet user-secrets init --id 50e6f746-1a06-4c3d-85e6-adc5aaf8d41f

# 產生對應的 generation.yml 檔案
# 需注意 generation.yml 檔案中的 directory 路徑格式
# 會將連線字串新增至 user-secrets 中
efg initialize -c
"Server=(localdb)\MSSQLLocalDB;Database=ContosoUniversity;Trusted_Connection=True"
-p SqlServer --id 50e6f746-1a06-4c3d-85e6-adc5aaf8d41f

# 列出當前的 user-secrets
dotnet user-secrets list

# 安裝相關套件
dotnet add package FluentValidation
dotnet add package AutoMapper
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# 生成程式碼
efg generate
```

註冊服務

```csharp
builder.Services.AddDbContext<ContosoUniversityEFGContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Generator"));
});

// 註冊 AutoMapper 服務並掃描所有的 Profile 類別
builder.Services.AddAutoMapper(typeof(Program).Assembly);
```

AutoMapper 使用方式，將 `Course` 實體類別映射到 `CourseReadModel` DTO 類別。

`ProjectTo` 會在 SQL 查詢中使用 SELECT 語句，只查詢 CourseReadModel 類別中的屬性，這樣可以確保只從資料庫中提取所需的資料，從而提高查詢效能。

AutoMapper 產生的 #region Generated xxx 區塊中的屬性，是根據映射規則自動生成的，不要手動修改這些屬性。

```csharp

public WeatherForecastController(ContosoUniversityEFGContext context,
        IMapper mapper)
{
    _context = context;
    _mapper = mapper;
}

var data = _context.Courses.ProjectTo<CourseReadModel>(_mapper.ConfigurationProvider);
```


### **使用 EFG 生成的目錄與程式碼**

* Data 資料夾
Data 資料夾主要負責資料存取層（Data Access Layer, DAL），包含與資料庫互動的程式碼。這裡的程式碼通常包括以下幾個部分：
    - `Entities`: 定義資料庫中的實體（Entity），這些實體對應於資料庫中的表格。
    - `Mapping`: 使用 Entity Framework Core 的 Fluent API 來配置實體與資料庫表格之間的映射關係。這些映射類別實現了 IEntityTypeConfiguration<T> 介面，並在 Configure 方法中定義表格名稱、主鍵、欄位屬性及關聯。
    - `Queries`: 包含擴充方法（Extension Methods），用於簡化和重用查詢邏輯。
    - `Context`: 定義資料庫上下文類別，繼承自 DbContext，並包含 DbSet<T> 屬性來表示資料庫中的表格。
* Domain 資料夾
Domain 資料夾主要負責業務邏輯層（Business Logic Layer, BLL），包含與業務邏輯相關的程式碼。這裡的程式碼通常包括以下幾個部分：
    - `Models`: 定義資料傳輸物件（DTO），這些物件用於在不同層之間傳遞資料。
    - `Mapping`: 使用 AutoMapper 來配置實體與 DTO 之間的映射關係。這些映射類別繼承自 AutoMapper.Profile，並在建構函式中定義映射規則。
    - `Validation`: 使用 FluentValidation 來定義 DTO 的驗證規則。這些驗證類別繼承自 AbstractValidator<T>，並在建構函式中定義驗證規則。


### **透過既有的程式碼產生對應的 Migration**

可以調整既有的連線設定，使更新資料庫時產生新的資料庫並包含完整的 DB Schema 

```bash
# 產生 Migration
dotnet ef migrations add {Migration Name}

# 更新資料庫
dotnet ef database update -v    # 套用變更到目前最新版本
dotnet ef database update 0     # 回覆所有資料庫移轉變更
dotnet ef database update <TO>  # 套用變更到指定版本

# 移除 Migration 預設會移除最後一個 Migration
# dotnet ef migrations remove

# 產生 SQL 腳本
# from 0 代表從第一個 Migration 開始
dotnet ef migrations script <FROM> <TO> -o output.sql

# 刪除資料庫
# dotnet ef database drop
```


### **透過 `aspnet-codegenerator` 產生 Controller**

安裝相關套件

```bash
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
```

產生 Controller

```bash
# -name: Controller 名稱
# -async: 使用非同步操作
# -m: 指定 Model 類別
# -dc: 指定 DbContext 類別
# -outDir: 輸出目錄
# -f: 強制覆蓋現有檔案
# -api: 產生 Web API Controller
dotnet aspnet-codegenerator controller -name CoursesController -async -m Course -dc ContosoUniversityContext -outDir Controllers -f -api
```


### **使用 DTO 類別替代實體模型**

在請求參數與回應時直接使用實體模型，可能會導致循環參考問題，或是回傳過多的資料，或是開放太多資料更新。

透過 DTO 類別，可以將實體模型中的部分屬性映射到 DTO 類別中，從而避免這些問題。

PUT 範例

```csharp
public async Task<IActionResult> PutCourse(int id, CourseUpdate course)
{
    var courseToUpdate = await _context.Courses.FindAsync(id);
    if (courseToUpdate == null)
    {
        return NotFound();
    }

    courseToUpdate.Credits = course.Credits;
    courseToUpdate.Title = course.Title;

    await _context.SaveChangesAsync();

    return NoContent();
}
```

POST 範例

```csharp
public async Task<ActionResult<Course>> PostCourse(CourseCreate courseToCreate)
{
    var course = new Course
    {
        Credits = courseToCreate.Credits,
        Title = courseToCreate.Title
    };

    _context.Courses.Add(course);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
}
```


### **使用 ORM 操作時不好的寫法**

```csharp
// 透過 Attach 方法將物件附加到 DbContext 中，並將狀態設定為 Modified
_context.Attach(course);
_context.Entry(course).State = EntityState.Modified;

// 透過 Update 一次更新全部屬性
_context.Update(course);

// 透過 Entry 方法附加物件並取得物件的狀態
_context.Entry(course).State = EntityState.Modified;
```


### **Attach、Entry**

如果是具有所產生索引鍵的實體類型，如果實體已設定其主鍵值，則會在狀態中 Unchanged 追蹤

如果未設定主鍵值，則會在 狀態中 Added 追蹤它。 這有助於確保只會插入新的實體

將狀態設定為 `EntityState.Modified`，則會做 `INSERT` 操作
* 若 `course` 未設定主鍵值，正常更新
* 若 `course` 設定主鍵值，則更新時可能會收到異常

```csharp
_context.Courses.Attach(course);
_context.Entry(course).State = EntityState.Added;
_context.SaveChanges();
```

將狀態設定為 `EntityState.Modified`，則會做 `Update` 操作
* 若 `course` 未設定主鍵值，則更新時可能會收到異常
* 若 `course` 設定主鍵值，則正常更新

```csharp
_context.Courses.Attach(course);
_context.Entry(course).State = EntityState.Modified;
_context.SaveChanges();
```

### **分頁作法**

1. 先做排序
2. 計算總筆數
3. 計算總頁數
4. 查詢分頁

```csharp
public async Task<ActionResult<IEnumerable<Course>>> GetCourses(int pageIndex = 1, int pageSize = 10)
{
    // 排序
    var courses = _context.Courses.OrderBy(c => c.CourseId).AsQueryable();

    // 計算總筆數
    var totalRecords = await courses.CountAsync();

    // 計算總頁數
    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

    // 查詢分頁
    var pagedCourses = await courses
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize).ToListAsync();
    
    return Ok(new
    {
        TotalPages = totalPages,
        TotalRecords = totalRecords,
        Data = pagedCourses
    });
}
```

### **Group Join**

沒有在結果中直接返回 `IEnumerable` 的集合。相反，對集合進行了過濾（`Where`）和選擇（`Select`），這些操作可以完全轉譯為 SQL

```csharp
public IActionResult GetDepartmentCoursesGroupJoin()
{
    var data = from d in _context.Departments
        join c in _context.Courses
        on d.DepartmentId equals c.DepartmentId into grouping
        select new
        {
            Department = new 
            {
                d.DepartmentId,
                d.Name,
                d.Budget,
                d.StartDate,
                Courses = grouping
                    .Where(c => c.Credits > 3)
                    .Select(c => new
                    {
                        c.CourseId,
                        c.Title,
                        c.Credits
                    })
            }
        };
    
    return Ok(data);
}
```

對應轉換成 SQL 語法

```sql
Executed DbCommand (42ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
SELECT [d].[DepartmentID], [d].[Name], [d].[Budget], [d].[StartDate], [c0].[CourseId], [c0].[Title], [c0].[Credits]
FROM [Department] AS [d]
LEFT JOIN (
    SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [c].[DepartmentID]
    FROM [Course] AS [c]
    WHERE [c].[Credits] > 3
) AS [c0] ON [d].[DepartmentID] = [c0].[DepartmentID]
ORDER BY [d].[DepartmentID]
```


### **Entity Framework Core 載入關聯資料的三種方法**

1. 預先載入 `Eager Loading`：使用 `Include` 方法一次性載入所有關聯資料。

```csharp
// 透過導覽屬性，直接對 Department 查詢
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
```

循環參考問題 (資料無法序列化成 JSON)

* 可以透過 [JsonIgnore] 解決「循環參考」問題，但不適用於 DB First 的開發情境，因為會調整到 EF Core 產生的程式碼。

```csharp
public partial class Course
{
    public int CourseId { get; set; }
    public string? Title { get; set; }

    [JsonIgnore] // <-- 加上這行
    public virtual Department Department { get; set; } = null!;
}
```

* 可以透過 DTO 來解決「循環參考」問題，自定義一個 DTO 類別，只包含需要的屬性。

效能問題 (資料量過大)

* 預先載入一個集合導覽屬性當資料量很大時容易對記憶體使用或是效能造成影響，此時可透過分割查詢(Split queries) 來優化查詢效率，避免因為 JOIN 的關係從資料庫回傳大量的結果集

```csharp
using (var context = new BloggingContext())
{
    var blogs = await context.Blogs
        .Include(blog => blog.Posts)
        .AsSplitQuery()  // <-- 加上這行
        .ToListAsync();
}
```

2. 明確載入 `Explicit Loading`：使用 `Load` 方法延遲載入關聯資料。

當你想精準的控制「關聯資料」的載入時機，就可以透過明確載入的 API 來自行載入導覽屬性的關聯資料

```csharp
public async Task<ActionResult<Course>> GetCourseWithDepartment(int id)
{
    var course = await _context.Courses.FindAsync(id);

    _context.Entry(course)
        .Reference(c => c.Department)
        .Load();

    return Ok(new
    {
        course.CourseId,
        course.Title,
        course.Credits,
        DepartmentName = course.Department.Name
    });
}
```

3. 延遲載入 `Lazy Loading`：使用 `virtual` 修飾符自動載入關聯資料。

容易引發經典的 N+1 問題， Entity Framework Core 預設不支援延遲載入，需要安裝 `Microsoft.EntityFrameworkCore.Proxies` 套件，並在 `OnConfiguring` 方法中啟用延遲載入。


### **使用 `TagWith` 註釋 SQL 查詢**

```csharp
var data = from item in _context.Courses.TagWith("Get Weather Forecast")
    join d in _context.Departments on item.DepartmentId equals d.DepartmentId
    // where d.StartDate.Date == DateTime.Parse("2015-03-21")
    select new
    {
        item.CourseId,
        item.Title,
        item.Credits,
        DepartmentName = d.Name
    };
```

```sql
-- Log
Executed DbCommand (86ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
-- Get Weather Forecast

SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
FROM [Course] AS [c]
INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
```


### **有效率的批次更新**

使用 `ExecuteUpdate` 方法進行批次更新，可以有效提高更新效率。

```csharp
[HttpPost("BatchUpdateCredits", Name = "PostBatchUpdateCredits")]
public async Task<IActionResult> PostBatchUpdateCredits()
{
    await _context.Courses.ExecuteUpdateAsync(setter => setter
        .SetProperty(c => c.Credits, c => c.Credits + 1));

    return NoContent();
}
```

對應產生的 SQL 查詢語句

```sql
Executed DbCommand (7ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
UPDATE [c]
SET [c].[Credits] = [c].[Credits] + 1
FROM [Course] AS [c]
```
