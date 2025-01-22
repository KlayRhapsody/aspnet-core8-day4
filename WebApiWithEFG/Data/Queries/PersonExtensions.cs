using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithEFG.Data.Queries;

public static partial class PersonExtensions
{
    #region Generated Extensions
    public static WebApiWithEFG.Data.Entities.Person? GetByKey(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Person> queryable, int id)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.Person> dbSet)
            return dbSet.Find(id);

        return queryable.FirstOrDefault(q => q.Id == id);
    }

    public static async System.Threading.Tasks.ValueTask<WebApiWithEFG.Data.Entities.Person?> GetByKeyAsync(this System.Linq.IQueryable<WebApiWithEFG.Data.Entities.Person> queryable, int id, System.Threading.CancellationToken cancellationToken = default)
    {
        if (queryable is null)
            throw new ArgumentNullException(nameof(queryable));

        if (queryable is DbSet<WebApiWithEFG.Data.Entities.Person> dbSet)
            return await dbSet.FindAsync(new object[] { id }, cancellationToken);

        return await queryable.FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    #endregion

}
