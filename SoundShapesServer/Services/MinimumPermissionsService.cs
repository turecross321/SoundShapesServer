using System.Net;
using System.Reflection;
using Bunkum.Listener.Request;
using Bunkum.Core.Authentication;
using Bunkum.Core.Database;
using Bunkum.Core.Responses;
using Bunkum.Core.Services;
using NotEnoughLogs;
using SoundShapesServer.Attributes;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Services;

public class MinimumPermissionsService : Service
{
    private readonly IAuthenticationProvider<GameSession>? _authProvider;
    
    internal MinimumPermissionsService(Logger logger, IAuthenticationProvider<GameSession>? authProvider) : base(logger)
    {
        _authProvider = authProvider;
    }

    public override Response? OnRequestHandled(ListenerContext context, MethodInfo method,
        Lazy<IDatabaseContext> database)
    {
        PermissionsType? minimumPermissions =
            method.GetCustomAttribute<MinimumPermissionsAttribute>()?.MinimumPermissions;
        if (minimumPermissions == null) return null;

        GameUser? user = _authProvider?.AuthenticateToken(context, database)?.User;
        
        if (user?.PermissionsType >= minimumPermissions)
            return null;

        return new Response(HttpStatusCode.Unauthorized);
    }
}