using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Queries;

public static partial class CourseExtensions
{
    #region Generated Extensions
    public static WebApiWithEFG.Data.Entities.Course? GetByKey(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Course> queryable, int courseID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.Course> dbSet)
            return dbSet.Find(courseID);

        return queryable.FirstOrDefault(q => q.CourseID == courseID);
    }

    public static async System.Threading.Tasks.ValueTask<WebApiWithEFG.Data.Entities.Course?> GetByKeyAsync(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Course> queryable, int courseID, System.Threading.CancellationToken cancellationToken = default)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.Course> dbSet)
            return await dbSet.FindAsync(new object[] { courseID }, cancellationToken);

        return await queryable.FirstOrDefaultAsync(q => q.CourseID == courseID, cancellationToken);
    }

    public static System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Course> ByDepartmentID(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Course> queryable, int departmentID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        return queryable.Where(q => q.DepartmentID == departmentID);
    }

    #endregion

}
