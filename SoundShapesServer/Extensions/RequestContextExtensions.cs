using Bunkum.Core;
using SoundShapesServer.Types;

namespace SoundShapesServer.Extensions;

public static class RequestContextExtensions
{
    public static PageData GetPageData(this RequestContext context)
    {
        return new PageData
        {
            Skip = context.QueryString["skip"].ToUInt() ?? 0,
            Take = context.QueryString["take"].ToUInt() ?? 9,
            MinimumCreationDate = context.QueryString["minimumCreationDate"].ToDate(),
            MaximumCreationDate = context.QueryString["maximumCreationDate"].ToDate(),
            ExcludeIds = context.QueryString.GetValues("exclude") ?? []
        };

    }
}