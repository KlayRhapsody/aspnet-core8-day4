using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Queries;

public static partial class EnrollmentExtensions
{
    #region Generated Extensions
    public static System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Enrollment> ByCourseID(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Enrollment> queryable, int courseID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        return queryable.Where(q => q.CourseID == courseID);
    }

    public static WebApiWithEFG.Data.Entities.Enrollment? GetByKey(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Enrollment> queryable, int enrollmentID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.Enrollment> dbSet)
            return dbSet.Find(enrollmentID);

        return queryable.FirstOrDefault(q => q.EnrollmentID == enrollmentID);
    }

    public static async System.Threading.Tasks.ValueTask<WebApiWithEFG.Data.Entities.Enrollment?> GetByKeyAsync(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Enrollment> queryable, int enrollmentID, System.Threading.CancellationToken cancellationToken = default)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.Enrollment> dbSet)
            return await dbSet.FindAsync(new object[] { enrollmentID }, cancellationToken);

        return await queryable.FirstOrDefaultAsync(q => q.EnrollmentID == enrollmentID, cancellationToken);
    }

    public static System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Enrollment> ByStudentID(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Enrollment> queryable, int studentID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        return queryable.Where(q => q.StudentID == studentID);
    }

    #endregion

}
