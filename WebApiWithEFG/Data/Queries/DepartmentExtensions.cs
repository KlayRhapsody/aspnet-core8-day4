using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Queries;

public static partial class DepartmentExtensions
{
    #region Generated Extensions
    public static WebApiWithEFG.Data.Entities.Department? GetByKey(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Department> queryable, int departmentID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.Department> dbSet)
            return dbSet.Find(departmentID);

        return queryable.FirstOrDefault(q => q.DepartmentID == departmentID);
    }

    public static async System.Threading.Tasks.ValueTask<WebApiWithEFG.Data.Entities.Department?> GetByKeyAsync(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Department> queryable, int departmentID, System.Threading.CancellationToken cancellationToken = default)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.Department> dbSet)
            return await dbSet.FindAsync(new object[] { departmentID }, cancellationToken);

        return await queryable.FirstOrDefaultAsync(q => q.DepartmentID == departmentID, cancellationToken);
    }

    public static System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Department> ByInstructorID(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Department> queryable, int? instructorID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        return queryable.Where(q => (q.InstructorID == instructorID || (instructorID == null && q.InstructorID == null)));
    }

    #endregion

}
