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

        GameUser? user = _authProvider?.AuthenticateUser(context, database);

        // if the endpoint doesn't need an account and a minimumPermissions hasn't been set, let them access it
        if (user == null && minimumPermissions == null)
            return null;

        // if minimumPermissions hasn't been set, assume it's Default
        minimumPermissions ??= PermissionsType.Default;
        
        // if the user has enough permissions, let them access it
        if (user?.PermissionsType >= minimumPermissions)
            return null;

        return new Response(HttpStatusCode.Unauthorized);
    }
}