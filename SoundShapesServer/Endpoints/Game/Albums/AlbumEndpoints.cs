using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Albums;
using SoundShapesServer.Responses.Game.Albums.LevelInfo;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Game.Albums;

public class AlbumEndpoints : EndpointGroup
{
    [GameEndpoint("~albums/~link:*.page", ContentType.Json)]
    public AlbumsWrapper GetAlbums(RequestContext context, GameDatabaseContext database, GameSession session)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        IQueryable<GameAlbum> albums = database.GetAlbums();

        return new AlbumsWrapper(albums, from, count);
    }

    [GameEndpoint("~album:{albumId}/~link:*.page", ContentType.Json)]
    public Response GetAlbumLevels
        (RequestContext context, GameDatabaseContext database, GameUser user, string albumId)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        string? order = context.QueryString["order"];

        GameAlbum? album = database.GetAlbumWithId(albumId);

        if (album == null) return HttpStatusCode.NotFound;

        IQueryable<GameLevel> levels = album.Levels.AsQueryable();
        
        if (order == "time:ascn")
            return new Response(new LevelsWrapper(levels, user, from, count, LevelOrderType.DoNotOrder), ContentType.Json);

        return new Response(new AlbumLevelInfosWrapper(user, album, levels, from, count), ContentType.Json);
    }
}