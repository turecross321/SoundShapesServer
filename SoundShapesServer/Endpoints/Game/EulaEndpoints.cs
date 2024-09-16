using System.Text.RegularExpressions;
using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Common.Types;
using SoundShapesServer.Common.Types.Config;
using SoundShapesServer.Common.Types.Database;
using SoundShapesServer.Database;

namespace SoundShapesServer.Endpoints.Game;

public partial class EulaEndpoints : EndpointGroup
{
    [GameEndpoint("{platform}/{publisher}/{language}/~eula.get")]
    public string GetEula(RequestContext context, GameDatabaseContext database, BunkumConfig bunkumConfig, DbToken token,
        DbUser user, ServerConfig config, string platform, string publisher, string language)
    {
        string eula = "";
        
        // this is included when we want to make sure that the eula is always shown (e.g. when showing the registration code)
        bool includeDate;
        
        switch (token.TokenType)
        {
            case TokenType.GameAccess:
                includeDate = false;
                eula = $"Welcome {user.Name}!";
                break;
            case TokenType.GameEula:
                includeDate = true;
                if (!user.FinishedRegistration)
                {
                    context.Logger.LogInfo(BunkumCategory.Authentication, "Creating initialize registration code for new user: " + user.Name);
                    DbCode code = database.CreateCode(user, CodeType.Registration);
                    
                    eula =
                        $"You currently do not have an account. To proceed, go to {bunkumConfig.ExternalUrl}/register and follow the instructions.\n" +
                        $"Your registration code is \"{code.Code}\".";
                }
                
                
                // todo: inform about bans, or if token auth / ip auth has been enabled etc.
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        eula = eula + "\n \n" + config.EulaText + "\n \n" + Licenses.AGPLNotice;
        if (includeDate)
            eula += "\n \n" + DateTimeOffset.UtcNow;

        return eula; // todo: investigate vita shit
    }
    

    [GeneratedRegex(@"^\/otg\/(\w+)\/(\w+)\/(\w+)\/~eula\.get$")]
    public static partial Regex EulaEndpointRegex();
}