using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Albums;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumEndpoints : EndpointGroup
{
    [ApiEndpoint("album/{id}")]
    [Authentication(false)]
    public ApiAlbumResponse? GetAlbum(RequestContext context, RealmDatabaseContext database, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        return album == null ? null : new ApiAlbumResponse(album);
    }
}