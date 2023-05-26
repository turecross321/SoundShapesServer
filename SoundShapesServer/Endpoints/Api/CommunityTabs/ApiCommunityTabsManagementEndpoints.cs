using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.CommunityTabs;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.CommunityTabs;

public class ApiCommunityTabsManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("communityTabs/create", Method.Post)]
    public Response CreateCommunityTab(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateCommunityTabRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        CommunityTab? createdCommunityTab = database.CreateCommunityTab(body, user);
        if (createdCommunityTab == null) 
            return new Response("There can't be more than 7 community tabs at a time.", ContentType.Plaintext, HttpStatusCode.Conflict);
        
        return new Response(new ApiCommunityTabResponse(createdCommunityTab), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("communityTabs/id/{id}/edit", Method.Post)]
    public Response EditCommunityTab(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateCommunityTabRequest body, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null) return HttpStatusCode.NotFound;

        CommunityTab editedCommunityTab = database.EditCommunityTab(communityTab, body, user);
        return new Response(new ApiCommunityTabResponse(editedCommunityTab), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("communityTabs/id/{id}/setThumbnail", Method.Post)]
    public Response SetCommunityTabThumbnail(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id, byte[] body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null) return HttpStatusCode.NotFound;

        return database.UploadCommunityTabResource(dataStore, communityTab, body);
    }

    [ApiEndpoint("communityTabs/id/{id}/remove", Method.Post)]
    public Response RemoveCommunityTab(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null) return HttpStatusCode.NotFound;
        
        database.RemoveCommunityTab(dataStore, communityTab);
        return HttpStatusCode.OK;
    }
}