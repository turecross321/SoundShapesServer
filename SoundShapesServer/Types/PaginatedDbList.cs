using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Types;

public class PaginatedDbList<TDb, TDbId> where TDb: IDbItem<TDbId>
{
    public static PaginatedDbList<TDb, TDbId> FromQueryable(IQueryable<TDb> items, PageData pageData)
    {
        if (pageData.MinimumCreationDate != null)
            items = items.Where(i => i.CreationDate >= pageData.MinimumCreationDate);

        if (pageData.MaximumCreationDate != null)
            items = items.Where(i => i.CreationDate < pageData.MaximumCreationDate);

        // Filter out items that have excluded ids
        items = pageData.ExcludeIds.Select(id => ParseId<TDbId>(id))
            .Aggregate(items, (current, parsedId) => 
                current.Where(i => !i.Id!.Equals((TDbId)parsedId)));

        uint totalSkip = pageData.Skip;
        uint totalTake = Math.Min(pageData.Take, MaxItems);
        
        PaginatedDbList<TDb, TDbId> paginatedList = new()
        {
            Items = items.Skip((int)totalSkip).Take((int)totalTake).AsEnumerable(),
            TotalItems = items.Count(),
            Skip = (int)totalSkip,
            Take = (int)totalTake,
        };

        return paginatedList;
    }

    private static object ParseId<TId>(string id)
    {
        Type type = typeof(TId);
        
        if (type == typeof(int))
        {
            return int.Parse(id);
        }

        if (type == typeof(Guid))
        {
            return Guid.Parse(id);
        }

        throw new ArgumentOutOfRangeException(nameof(id), "Tried to parse unsupported id!");
    }

    private const int MaxItems = 100;
    
    public required IEnumerable<TDb> Items { get; init; }
    public required int TotalItems { get; init; }
    public required int Skip { get; init; }
    public required int Take { get; init; }
    public int? PreviousPageIndex()
    {
        int? previousToken;
        if (Skip > 0)
        {
            previousToken = Math.Max(Skip - Take, 0);
        }
        else
        {
            previousToken = null;
        }
        
        return previousToken;
    }
    
    public int? NextPageIndex()
    {
        int? nextToken;
        if (TotalItems <= Skip + Take)
        {
            nextToken = null;
        }
        else nextToken = Skip + Take;
        
        return nextToken;
    }
}