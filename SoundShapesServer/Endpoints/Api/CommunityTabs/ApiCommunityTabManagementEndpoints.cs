using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.CommunityTabs;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.CommunityTabs;

public class ApiCommunityTabManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("communityTabs/create", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    public Response CreateCommunityTab(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateCommunityTabRequest body)
    {
        CommunityTab? createdCommunityTab = database.CreateCommunityTab(body, user);
        if (createdCommunityTab == null) 
            return new Response("There can't be more than 4 custom community tabs at a time.", ContentType.Plaintext, HttpStatusCode.Conflict);
        
        return new Response(new ApiCommunityTabResponse(createdCommunityTab), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("communityTabs/id/{id}/edit", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits community tab with specified ID.")]
    public Response EditCommunityTab(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateCommunityTabRequest body, string id)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null) return HttpStatusCode.NotFound;

        CommunityTab editedCommunityTab = database.EditCommunityTab(communityTab, body, user);
        return new Response(new ApiCommunityTabResponse(editedCommunityTab), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("communityTabs/id/{id}/setThumbnail", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets thumbnail of community tab with specified ID.")]
    public Response SetCommunityTabThumbnail(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id, byte[] body)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null) return HttpStatusCode.NotFound;

        return database.UploadCommunityTabResource(dataStore, communityTab, body);
    }

    [ApiEndpoint("communityTabs/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes community tab with specified ID.")]
    public Response RemoveCommunityTab(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null) return HttpStatusCode.NotFound;
        
        database.RemoveCommunityTab(dataStore, communityTab);
        return HttpStatusCode.OK;
    }
}