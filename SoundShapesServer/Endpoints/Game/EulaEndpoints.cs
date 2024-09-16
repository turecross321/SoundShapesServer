using System.Text.RegularExpressions;
using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Config;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Endpoints.Game;

public partial class EulaEndpoints : EndpointGroup
{
    private const string AGPLNotice = """
                                      This program is free software: you can redistribute it and/or modify
                                      it under the terms of the GNU Affero General Public License as published
                                      by the Free Software Foundation, either version 3 of the License, or
                                      (at your option) any later version.

                                      This program is distributed in the hope that it will be useful,
                                      but WITHOUT ANY WARRANTY; without even the implied warranty of
                                      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
                                      GNU Affero General Public License for more details.

                                      You should have received a copy of the GNU Affero General Public License
                                      along with this program.  If not, see <https://www.gnu.org/licenses/>.
                                      """;
    
    [GameEndpoint("{platform}/{publisher}/{language}/~eula.get")]
    public string GetEula(RequestContext context, GameDatabaseContext database, BunkumConfig bunkumConfig, DbToken token,
        DbUser user, ServerConfig config, string platform, string publisher, string language)
    {
        string eula = "";
        
        // this is included when we want to make sure that the eula is always shown (eg. when showing the registration code)
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
                    context.Logger.LogInfo(BunkumCategory.Authentication, "Creating set email token for new user: " + user.Name);
                    DbCodeToken codeToken = database.CreateCodeToken(user, CodeTokenType.SetEmail);
                    
                    eula =
                        $"You currently do not have an account. To proceed, go to {bunkumConfig.ExternalUrl}/register and follow the instructions.\n" +
                        $"Your registration code is \"{codeToken.Code}\".";
                    break;
                }
                
                
                // todo: inform about bans, or if token auth / ip auth has been enabled etc.
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        eula = eula + "\n \n" + config.EulaText + "\n \n" + AGPLNotice;
        if (includeDate)
            eula += "\n \n" + DateTimeOffset.UtcNow;

        return eula; // todo: investigate vita shit
    }
    

    [GeneratedRegex(@"^\/otg\/(\w+)\/(\w+)\/(\w+)\/~eula\.get$")]
    public static partial Regex EulaEndpointRegex();
}