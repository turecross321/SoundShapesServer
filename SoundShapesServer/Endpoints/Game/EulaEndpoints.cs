using System.Text.RegularExpressions;
using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Config;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Endpoints.Game;

public partial class EulaEndpoints : EndpointGroup
{
    [GameEndpoint("{platform}/{publisher}/{language}/~eula.get")]
    public string GetEula(RequestContext context, GameDatabaseContext database, BunkumConfig bunkumConfig, DbToken token,
        DbUser user, ServerConfig config, string platform, string publisher, string language)
    {
        string eula = "";
        
        // this is included when we want to make sure that the eula is always shown (e.g. when showing the registration code)
        bool includeDate = false;

        if (token.TokenType == TokenType.GameAccess)
        {
            includeDate = false;
            eula = $"Welcome {user.Name}!";
        }
        else if (!user.FinishedRegistration)
        {
            context.Logger.LogInfo(BunkumCategory.Authentication, "Creating initialize registration code for new user: " + user.Name);
            DbCode code = database.CreateCode(user, CodeType.Registration);
                    
            eula =
                $"You currently do not have an account. To proceed, go to {config.WebsiteUrl}/register and follow the instructions.\n" +
                $"Your registration code is \"{code.Code}\".";
        }
        else
        {
            eula = "Your session has not been authorized.\n\n" +
                   $"To proceed, go to {config.WebsiteUrl}/authorization " +
                   "and perform one of the following actions:\n\n";

            if (token.GenuineNpTicket == true)
            {
                switch (token.Platform)
                {
                    case PlatformType.RPCS3:
                        eula += "- Enable RPCN Authorization\n";
                        break;
                    case PlatformType.PS3 or PlatformType.PS4 or PlatformType.PSVita:
                        eula += "- Enable PSN Authorization\n";
                        break;
                }
            }

            eula += "- Enable IP Authorization";
        }

        eula = eula + "\n \n" + config.EulaText + "\n \n" + Licenses.AGPLNotice;
        if (includeDate)
            eula += "\n \n" + DateTimeOffset.UtcNow;

        return eula;
    }
    

    [GeneratedRegex(@"^\/otg\/(\w+)\/(\w+)\/(\w+)\/~eula\.get$")]
    public static partial Regex EulaEndpointRegex();
}