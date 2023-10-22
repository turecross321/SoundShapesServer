using System.Linq.Expressions;

namespace SoundShapesServer.Extensions.Queryable;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderByDynamic<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector,
        bool descending)
    {
        return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
    }

    private const int MaxItems = 100;

    public static (T[], int) Paginate<T>(this IQueryable<T> entries, int from, int count)
    {
        return (entries.AsEnumerable().Skip(from).Take(Math.Min(count, MaxItems)).ToArray(), entries.Count());
    }
}