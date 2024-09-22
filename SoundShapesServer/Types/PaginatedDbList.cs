using SoundShapesServer.Extensions;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

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

        int totalSkip = pageData.Skip;
        int totalTake = Math.Min(pageData.Take, MaxItems);
        
        if (pageData.FromId != null)
        {
            object parsedId = ParseId<TDbId>(pageData.FromId);
            
            int? idIndex = items
                .Select((item, idx) => new { Item = item, Index = idx })
                .Where(x => x.Item.Id!.Equals((TDbId)parsedId))
                .Select(x => x.Index)
                .FirstOrDefault();

            if (idIndex != null)
                totalSkip += (int)idIndex;
        }
        
        
        PaginatedDbList<TDb, TDbId> paginatedList = new()
        {
            Items = items.Skip(totalSkip).Take(totalTake).AsEnumerable(),
            TotalItems = items.Count(),
            Skip = totalSkip,
            Take = totalTake,
        };

        paginatedList.NextPageItemId = paginatedList.NextPageIndex() != null 
            ? items.ElementAt((int)paginatedList.NextPageIndex()!).Id?.ToString() 
            : null;
        
        paginatedList.PreviousPageItemId = paginatedList.PreviousPageIndex() != null 
            ? items.ElementAt((int)paginatedList.PreviousPageIndex()!).Id?.ToString() 
            : null;

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

    public string? NextPageItemId { get; set; }
    public string? PreviousPageItemId { get; set; }
    
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