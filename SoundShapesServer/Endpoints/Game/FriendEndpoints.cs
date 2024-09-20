using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Protocols.Http;

namespace SoundShapesServer.Endpoints.Game;

public class FriendEndpoints : EndpointGroup
{
    [Authentication(false)]
    [GameEndpoint("~identity:{id}/~friends.all")]
    public Response GetFriends(RequestContext context)
    {
        // Todo: does the game actually do anything with this? should we send it the users actual friends list? following list?
        
        return OK;
    }
    
    [AllowEmptyBody]
    [Authentication(false)]
    [GameEndpoint("identity/person/{id}/data/psn/friends-list", HttpMethods.Post)]
    public Response UploadFriendsList(RequestContext context, string body)
    {
        IEnumerable<string> friendNames = body.Split("\n");
        
        // Todo: Follow friends
        // Note that authentication is false... we probably want to fix that
        // todo: is this only new friends or is it all friends?
        
        return OK;
    }
}