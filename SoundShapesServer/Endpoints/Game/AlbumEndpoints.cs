using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Albums;
using SoundShapesServer.Responses.Game.Albums.LevelInfo;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game;

public class AlbumEndpoints : EndpointGroup
{
    [GameEndpoint("~albums/~link:*.page")]
    public AlbumsWrapper GetAlbums(RequestContext context, GameDatabaseContext database, GameSession session)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        (GameAlbum[] albums, int totalAlbums) = database.GetAlbums(AlbumOrderType.CreationDate, true, from, count);

        return new AlbumsWrapper(albums, totalAlbums, from, count);
    }

    [GameEndpoint("~album:{albumId}/~link:*.page")]
    public Response GetAlbumLevels
        (RequestContext context, GameDatabaseContext database, GameUser user, string albumId)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);
        string? order = context.QueryString["order"];

        GameAlbum? album = database.GetAlbumWithId(albumId);

        if (album == null) return HttpStatusCode.NotFound;

        (GameLevel[] levels, int totalLevels) = database.GetLevels(LevelOrderType.Difficulty, true, new LevelFilters(inAlbum: album), from, count);

        if (order == "time:ascn")
            return new Response(new LevelsWrapper(levels, user, totalLevels, from, count), ContentType.Json);

        return new Response(new AlbumLevelInfosWrapper(user, album, levels, from, count), ContentType.Json);
    }
    
    [GameEndpoint("{platform}/{publisher}/{language}/~translation.get")]
    public Response GetTranslatedLinerNotes(RequestContext context, string platform, string publisher, string language)
    {
        // This is for Album Translations
        return new Response(HttpStatusCode.OK);
    }
}