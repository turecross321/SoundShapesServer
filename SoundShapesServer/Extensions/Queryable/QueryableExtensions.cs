using System.Linq.Expressions;

namespace SoundShapesServer.Extensions.Queryable;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderByDynamic<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector,
        bool descending)
    {
        return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
    }
    
    private static readonly int _maxItems = 100;
    public static T[] Paginate<T>(this IQueryable<T> entries, int from, int count)
    {
        return entries.AsEnumerable().Skip(from).Take(Math.Min(count, _maxItems)).ToArray();
    }
}