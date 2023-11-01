using System.Linq.Expressions;

namespace SoundShapesServer.Extensions.Queryable;

public static class QueryableExtensions
{
    public static IOrderedQueryable<T> OrderByDynamic<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> keySelector,
        bool descending)
    {
        return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
    }
    
    public static IOrderedQueryable<T> ThenByDynamic<T, TKey>(this IOrderedQueryable<T> source, Expression<Func<T, TKey>> keySelector, bool descending)
    {
        return descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
    }
}