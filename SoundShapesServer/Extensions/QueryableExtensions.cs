using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Extensions;

public static class QueryableExtensions
{
    public static PaginatedDbList<TDb, Guid> PaginateWithGuidId<TDb>(this IQueryable<TDb> items, PageData pageData)
        where TDb : IDbItem<Guid>
    {
        return PaginateInternal<TDb, Guid>(items, pageData);
    }

    public static PaginatedDbList<TDb, int> PaginateWithIntId<TDb>(this IQueryable<TDb> items, PageData pageData)
        where TDb : IDbItem<int>
    {
        return PaginateInternal<TDb, int>(items, pageData);
    }

    private static PaginatedDbList<TDb, TDbId> PaginateInternal<TDb, TDbId>(IQueryable<TDb> items, PageData pageData)
        where TDb : IDbItem<TDbId>
    {
        return PaginatedDbList<TDb, TDbId>.FromQueryable(items, pageData);
    }
}