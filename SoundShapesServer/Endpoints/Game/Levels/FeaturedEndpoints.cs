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
        new CommunityTab(buttonLabel: "New Levels", query: "newest",
            panelDescription: "Check here daily for the latest cool levels! Always new stuff to check out!",
            imageUrl: "", panelTitle: "New Levels"),
        
        new CommunityTab(buttonLabel: "Random Levels", query: "random",
            panelDescription: "Check here for random levels! Gets reshuffled every day!", imageUrl: "",
            panelTitle: "Random Levels"),
        
        new CommunityTab(buttonLabel: "Large Levels", query: "largest",
            panelDescription: "Check here for the largest levels, sorted by file size!", imageUrl: "",
            panelTitle: "Large Levels"),
        
        new CommunityTab(buttonLabel: "Hard Levels", query: "hardest",
            panelDescription: "Check here for the hardest levels, sorted by difficulty!", imageUrl: "",
            panelTitle: "Hard Levels"),
        
        new CommunityTab(buttonLabel: "Top Levels", query: "top",
            panelDescription: "Check here for the most played levels!", imageUrl: "", panelTitle: "Top Levels")
    };
    
    [GameEndpoint("global/featured/~metadata:*.get")]
    [GameEndpoint("global/featured/{language}/~metadata:*.get", ContentType.Plaintext)]
    public string GlobalFeatured(RequestContext context, RealmDatabaseContext database, string? language)
    {
        return FeaturedHelper.SerializeCommunityTabs(_communityTabs);
    }
}