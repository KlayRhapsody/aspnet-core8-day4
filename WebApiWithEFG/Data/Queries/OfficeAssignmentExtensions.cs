using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Queries;

public static partial class OfficeAssignmentExtensions
{
    #region Generated Extensions
    public static WebApiWithEFG.Data.Entities.OfficeAssignment? GetByKey(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.OfficeAssignment> queryable, int instructorID)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.OfficeAssignment> dbSet)
            return dbSet.Find(instructorID);

        return queryable.FirstOrDefault(q => q.InstructorID == instructorID);
    }

    public static async System.Threading.Tasks.ValueTask<WebApiWithEFG.Data.Entities.OfficeAssignment?> GetByKeyAsync(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.OfficeAssignment> queryable, int instructorID, System.Threading.CancellationToken cancellationToken = default)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.OfficeAssignment> dbSet)
            return await dbSet.FindAsync(new object[] { instructorID }, cancellationToken);

        return await queryable.FirstOrDefaultAsync(q => q.InstructorID == instructorID, cancellationToken);
    }

    #endregion

}
