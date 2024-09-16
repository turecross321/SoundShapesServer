﻿using Bunkum.Core.Authentication;
using Bunkum.Core.Database;
using Bunkum.Listener.Request;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Endpoints.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer;

public class GameAuthenticationProvider: IAuthenticationProvider<DbToken>
{
    public DbToken? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        // check the session header. this is what the game uses
        string? tokenId = request.RequestHeaders["X-OTG-Identity-SessionId"];

        // check the authorization header. this is what the api uses.
        tokenId ??= request.RequestHeaders["Authorization"];

        // we dont have a token if null, so bail 
        if (tokenId == null) return null;

        if (!Guid.TryParse(tokenId, out Guid parsedId))
            return null;

        GameDatabaseContext database = (GameDatabaseContext)db.Value;

        DbToken? token = database.GetTokenWithId(parsedId);
        if (token == null) return null;

        if (token.ExpiryDate < DateTimeOffset.UtcNow)
        {
            database.RemoveToken(token);
            return null;
        }

        string uriPath = request.Uri.AbsolutePath;
        
        if (uriPath.StartsWith(GameEndpointAttribute.RoutePrefix)
            && token.TokenType == TokenType.GameAccess)
        { 
            return token;
        }

        if (token.TokenType == TokenType.GameEula && EulaEndpoints.EulaEndpointRegex().IsMatch(uriPath))
        {
            return token;
        }
        
        if (uriPath.StartsWith(ApiEndpointAttribute.RoutePrefix) && token.TokenType == TokenType.ApiAccess)
        {
            return token;
        }

        return null;
    }
}