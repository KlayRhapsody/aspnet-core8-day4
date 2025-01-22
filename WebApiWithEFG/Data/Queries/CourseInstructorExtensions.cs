using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Queries;

public static partial class CourseInstructorExtensions
{
    #region Generated Extensions
    public static System.Linq.IQueryable<WebApiWithEFG.Data.Entities.CourseInstructor> ByCourseID(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.CourseInstructor> queryable, int courseID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        return queryable.Where(q => q.CourseID == courseID);
    }

    public static WebApiWithEFG.Data.Entities.CourseInstructor? GetByKey(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.CourseInstructor> queryable, int courseID, int instructorID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.CourseInstructor> dbSet)
            return dbSet.Find(courseID, instructorID);

        return queryable.FirstOrDefault(q => q.CourseID == courseID
            && q.InstructorID == instructorID);
    }

    public static async System.Threading.Tasks.ValueTask<WebApiWithEFG.Data.Entities.CourseInstructor?> GetByKeyAsync(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.CourseInstructor> queryable, int courseID, int instructorID, System.Threading.CancellationToken cancellationToken = default)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.CourseInstructor> dbSet)
            return await dbSet.FindAsync(new object[] { courseID, instructorID }, cancellationToken);

        return await queryable.FirstOrDefaultAsync(q => q.CourseID == courseID
            && q.InstructorID == instructorID, cancellationToken);
    }

    public static System.Linq.IQueryable<WebApiWithEFG.Data.Entities.CourseInstructor> ByInstructorID(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.CourseInstructor> queryable, int instructorID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        return queryable.Where(q => q.InstructorID == instructorID);
    }

    #endregion

}
