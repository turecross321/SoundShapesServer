using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Albums;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Game.Albums;

public class AlbumEndpoints : EndpointGroup
{
    [GameEndpoint("~albums/~link:*.page", ContentType.Json)]
    public AlbumsWrapper GetAlbums(RequestContext context, RealmDatabaseContext database, GameSession session)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        IQueryable<GameAlbum> albums = database.GetAlbums();
        AlbumsWrapper response = AlbumHelper.AlbumsToAlbumsWrapper(session.Id, albums, from, count);

        return response;
    }

    [GameEndpoint("~album:{albumId}/~link:*.page", ContentType.Json)]
    public Response GetAlbumLevels
        (RequestContext context, RealmDatabaseContext database, GameUser user, string albumId)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        string? order = context.QueryString["order"];

        GameAlbum? album = database.GetAlbumWithId(albumId);

        if (album == null) return HttpStatusCode.NotFound;

        if (order == "time:ascn")
        {
            IQueryable<GameLevel> levels = database.AlbumLevels(album);
            LevelsWrapper response = LevelHelper.LevelsToLevelsWrapper(levels, user, from, count);

            return new Response(response, ContentType.Json);
        }

        return new Response(database.GetAlbumsLevelsInfo(user, album, from, count), ContentType.Json);
    }
}