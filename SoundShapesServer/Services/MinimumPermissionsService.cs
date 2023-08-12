using System.Net;
using System.Reflection;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Database;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Services;
using NotEnoughLogs;
using SoundShapesServer.Attributes;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Services;

public class MinimumPermissionsService : Service
{
    private readonly IAuthenticationProvider<GameUser, GameSession>? _authProvider;
    
    internal MinimumPermissionsService(LoggerContainer<BunkumContext> logger, IAuthenticationProvider<GameUser, GameSession>? authProvider) : base(logger)
    {
        _authProvider = authProvider;
    }

    public override Response? OnRequestHandled(ListenerContext context, MethodInfo method,
        Lazy<IDatabaseContext> database)
    {
        PermissionsType? minimumPermissions =
            method.GetCustomAttribute<MinimumPermissionsAttribute>()?.MinimumPermissions;
        if (minimumPermissions == null) return null;

        GameUser? user = _authProvider?.AuthenticateUser(context, database);
        
        if (user?.PermissionsType >= minimumPermissions)
            return null;

        return new Response(HttpStatusCode.Unauthorized);
    }
}