namespace SoundShapesServer.Common.Types;

public class PaginatedList<TObject> where TObject : class
{
    private const int MaxItems = 100;

    public PaginatedList(IQueryable<TObject> items, int from, int count)
    {
        Items = items.AsEnumerable().Skip(from).Take(Math.Min(count, MaxItems)).ToArray();
        TotalItems = items.Count();
        From = from;
    }

    public PaginatedList(IEnumerable<TObject> items, int from, int count)
    {
        IEnumerable<TObject> enumerable = items as TObject[] ?? items.ToArray();

        Items = enumerable.Skip(from).Take(Math.Min(count, MaxItems)).ToArray();
        TotalItems = enumerable.Count();
        From = from;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private PaginatedList() {
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }

    public IEnumerable<TObject> Items { get; init; }
    public int TotalItems { get; init; }
    public int From { get; init; }

    public static PaginatedList<TNewObject> SwapItems<TNewObject, TOldObject>(PaginatedList<TOldObject> oldList,
        IEnumerable<TNewObject> newItems)
        where TNewObject : class
        where TOldObject : class
    {
        return new PaginatedList<TNewObject>
        {
            Items = newItems,
            TotalItems = oldList.TotalItems,
            From = oldList.From
        };
    }
}