using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class FeaturedEndpoints : EndpointGroup
{
    // WARNING: More than 7 community tabs will CRASH the game.
    private readonly List<CommunityTab> _communityTabs = new ()
    {
        new CommunityTab
        {
            ButtonLabel = "New Levels",
            Query = "newest",
            PanelDescription = "Check here daily for the latest cool levels! Always new stuff to check out!",
            ImageUrl = "",
            PanelTitle = "New Levels"
        },
        new CommunityTab
        {
            ButtonLabel = "Random Levels",
            Query = "random",
            PanelDescription = "Check here for random levels! Gets reshuffled every day!",
            ImageUrl = "",
            PanelTitle = "Random Levels"
        },
        new CommunityTab
        {
            ButtonLabel = "Large Levels",
            Query = "largest",
            PanelDescription = "Check here for the largest levels, sorted by file size!",
            ImageUrl = "",
            PanelTitle = "Large Levels"
        },
        new CommunityTab
        {
            ButtonLabel = "Hard Levels",
            Query = "hardest",
            PanelDescription = "Check here for the hardest levels, sorted by difficulty!",
            ImageUrl = "",
            PanelTitle = "Hard Levels"
        },
        new CommunityTab
        {
            ButtonLabel = "Top Levels",
            Query = "top",
            PanelDescription = "Check here for the most played levels!",
            ImageUrl = "",
            PanelTitle = "Top Levels"
        }
    };
    
    [GameEndpoint("global/featured/~metadata:*.get")]
    [GameEndpoint("global/featured/{language}/~metadata:*.get", ContentType.Plaintext)]
    public string GlobalFeatured(RequestContext context, RealmDatabaseContext database, string? language)
    {
        return FeaturedHelper.SerializeCommunityTabs(_communityTabs);
    }
}