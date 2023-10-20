using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions.Queryable;

public static class NewsQueryableExtensions
{
    public static IQueryable<NewsEntry> FilterNews(this IQueryable<NewsEntry> entries, NewsFilters filters)
    {
        if (filters.Language != null)
        {
            entries = entries.Where(e => e.Language == filters.Language);
        }

        if (filters.Authors != null)
        {
            IEnumerable<NewsEntry> tempResponse = new List<NewsEntry>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (GameUser author in filters.Authors)
            {
                tempResponse = tempResponse.Concat(entries.Where(e=> e.Author == author));
            }

            entries = tempResponse.AsQueryable();
        }

        return entries;
    }
    
    public static IQueryable<NewsEntry> OrderNews(this IQueryable<NewsEntry> entries, NewsOrderType order, bool descending)
    {
        return order switch
        {
            NewsOrderType.CreationDate => entries.OrderByDynamic(e => e.CreationDate, descending),
            NewsOrderType.ModificationDate => entries.OrderByDynamic(e => e.ModificationDate, descending),
            NewsOrderType.CharacterCount => entries.OrderByDynamic(e => e.CharacterCount, descending),
            _ => entries.OrderNews(NewsOrderType.CreationDate, descending)
        };
    }
}