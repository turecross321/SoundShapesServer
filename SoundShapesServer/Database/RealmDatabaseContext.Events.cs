using Realms;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<GameEvent> GetEvents()
    {
        return _realm.All<GameEvent>();
    }

    private void CreateEvent(GameUser actor, EventType eventType, GameUser? user = null, GameLevel? level = null, LeaderboardEntry? leaderboardEntry = null)
    {
        GameEvent eventObject = new (actor, user, level, leaderboardEntry, eventType);
        
        GameEvent? previousEvent = _realm
            .All<GameEvent>()
            .AsEnumerable()
            .Where(e => e.Actor.Id == eventObject.Actor.Id)
            .FirstOrDefault(e => 
                e.EventType == eventObject.EventType 
                && Equals(e.ContentUser, user) 
                && Equals(e.ContentLevel, level) 
                && Equals(e.ContentLeaderboardEntry, leaderboardEntry));

        if (previousEvent != null) return;
        
        _realm.Write(() =>
        {
            _realm.Add(eventObject);
        });
    }

    public GameEvent? GetEventWithId(string id)
    {
        return _realm.All<GameEvent>().FirstOrDefault(e => e.Id == id);
    }
    
    public void RemoveEvent(GameEvent eventObject)
    {
        _realm.Write(() => _realm.Remove(eventObject));
    }
}