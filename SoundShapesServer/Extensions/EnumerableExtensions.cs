namespace SoundShapesServer.Extensions;

public static class EnumerableExtensions
{
    public static IOrderedEnumerable<T> OrderByDynamic<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, bool descending)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (keySelector == null)
        {
            throw new ArgumentNullException(nameof(keySelector));
        }

        return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
    }
}