using System.Net;
using System.Reflection;
using Bunkum.Core.Authentication;
using Bunkum.Core.Database;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Core.Services;
using Bunkum.Listener.Request;
using NotEnoughLogs;
using SoundShapesServer.Attributes;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Services;

public class MinimumPermissionsService : EndpointService
{
    private readonly IAuthenticationProvider<GameToken>? _authProvider;
    
    internal MinimumPermissionsService(Logger logger, IAuthenticationProvider<GameToken>? authProvider) : base(logger)
    {
        _authProvider = authProvider;
    }

    public override Response? OnRequestHandled(ListenerContext context, MethodInfo method,
        Lazy<IDatabaseContext> database)
    {
        PermissionsType? minimumPermissions =
            method.GetCustomAttribute<MinimumPermissionsAttribute>()?.MinimumPermissions;
        
        // if no authentication is required, let them through
        if (method.GetCustomAttribute<AuthenticationAttribute>()?.Required == false)
            return null;
        
        GameUser? user = _authProvider?.AuthenticateToken(context, database)?.User;

        // if minimumPermissions hasn't been set, assume it's Default
        minimumPermissions ??= PermissionsType.Default;
        
        // if the user has enough permissions, let them access it
        if ((user?.PermissionsType ?? PermissionsType.Banned) >= minimumPermissions)
            return null;

        return new Response(HttpStatusCode.Unauthorized);
    }
}