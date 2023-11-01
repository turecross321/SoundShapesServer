using SoundShapesServer.Responses;

namespace SoundShapesServer.Types;

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
        Items = items.Skip(from).Take(Math.Min(count, MaxItems)).ToArray();
        TotalItems = items.Count();
        From = from;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private PaginatedList()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public IEnumerable<TObject> Items { get; set; }
    public int TotalItems { get; set; }
    public int From { get; set; }

    public static PaginatedList<TNewObject> FromOldList<TNewObject, TOldObject>(PaginatedList<TOldObject> oldList)
        where TNewObject : class, IDataConvertableFrom<TNewObject, TOldObject>
        where TOldObject : class
    {
        return new PaginatedList<TNewObject>
        {
            Items = TNewObject.FromOldList(oldList.Items),
            TotalItems = oldList.TotalItems,
            From = oldList.From
        };
    }
}