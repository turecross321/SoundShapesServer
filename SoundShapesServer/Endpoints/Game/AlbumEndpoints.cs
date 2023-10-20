using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Responses.Game.Albums;
using SoundShapesServer.Responses.Game.Albums.LevelInfo;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game;

public class AlbumEndpoints : EndpointGroup
{
    [GameEndpoint("~albums/~link:*.page")]
    public ListResponse<AlbumResponse> GetAlbums(RequestContext context, GameDatabaseContext database, GameToken token)
    {
        (int from, int count, bool _) = context.GetPageData();

        (GameAlbum[] albums, int totalAlbums) = database.GetPaginatedAlbums(AlbumOrderType.CreationDate, true, from, count);

        return new ListResponse<AlbumResponse>(albums.Select(a => new AlbumResponse(a)), totalAlbums, from, count);
    }

    [GameEndpoint("~album:{albumId}/~link:*.page")]
    public Response GetAlbumLevels
        (RequestContext context, GameDatabaseContext database, GameUser user, string albumId)
    {
        (int from, int count, bool _) = context.GetPageData();
        string? order = context.QueryString["order"];

        GameAlbum? album = database.GetAlbumWithId(albumId);

        if (album == null) return HttpStatusCode.NotFound;

        (GameLevel[] levels, int totalLevels) = database.GetPaginatedLevels(LevelOrderType.Difficulty, true, new LevelFilters{InAlbum = album}, from, count, user);

        if (order == "time:ascn")
            return new Response(new ListResponse<LevelResponse>(levels.Select(l=>new LevelResponse(l, user)), totalLevels, from, count), ContentType.Json);

        return new Response(new ListResponse<AlbumLevelInfoResponse>(levels.Select(l => new AlbumLevelInfoResponse(l, album, user)), totalLevels, from, count), ContentType.Json);
    }
    
    [GameEndpoint("{platform}/{publisher}/{language}/~translation.get")]
    public Response GetTranslatedLinerNotes(RequestContext context, string platform, string publisher, string language)
    {
        // This is for Album Translations
        return new Response(HttpStatusCode.OK);
    }
}