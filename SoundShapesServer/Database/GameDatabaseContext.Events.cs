using MongoDB.Bson;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private void CreateEvent(GameUser actor, EventType eventType, PlatformType platformType, EventDataType dataType,
        string dataId)
    {
        GameEvent gameEvent = new()
        {
            Actor = actor,
            DataType = dataType,
            DataId = dataId,
            EventType = eventType,
            CreationDate = DateTimeOffset.UtcNow,
            PlatformType = platformType
        };

        if (eventType != EventType.ScoreSubmission)
            // return if there are any pre-existing identical events
            if (_realm.All<GameEvent>().FirstOrDefault(e =>
                    e.Actor == actor && gameEvent._EventType == (int)eventType && e.DataId == dataId) != null)
                return;

        _realm.Write(() =>
        {
            _realm.Add(gameEvent);
            actor.EventsCount = actor.Events.Count();
        });

        CreateNotification(gameEvent);
    }

    public void RemoveEvent(GameEvent eventObject)
    {
        _realm.Write(() =>
        {
            _realm.Remove(eventObject);
            eventObject.Actor.EventsCount = eventObject.Actor.Events.Count();
        });
    }

    private void RemoveEventsOnUser(GameUser user)
    {
        IQueryable<GameEvent> events = _realm.All<GameEvent>()
            .Where(e => e._DataType == (int)EventDataType.User && e.DataId == user.Id);
        _realm.Write(() => { _realm.RemoveRange(events); });
    }

    private void RemoveEventsOnLevel(GameLevel level)
    {
        foreach (LeaderboardEntry entry in level.LeaderboardEntries)
            RemoveEventsOnLeaderboardEntry(entry);

        IQueryable<GameEvent> events = _realm.All<GameEvent>()
            .Where(e => e._DataType == (int)EventDataType.Level && e.DataId == level.Id);
        _realm.Write(() => { _realm.RemoveRange(events); });
    }

    private void RemoveEventsOnAlbum(GameAlbum album)
    {
        string id = album.Id.ToString()!;
        IQueryable<GameEvent> events = _realm.All<GameEvent>()
            .Where(e => e._DataType == (int)EventDataType.Album && e.DataId == id);
        _realm.Write(() => { _realm.RemoveRange(events); });
    }

    private void RemoveEventsOnLeaderboardEntry(LeaderboardEntry entry)
    {
        string id = entry.Id.ToString()!;

        _realm.Write(() =>
        {
            _realm.RemoveRange(_realm.All<GameEvent>()
                .Where(e => e._DataType == (int)EventDataType.LeaderboardEntry && e.DataId == id));
        });
    }

    public GameEvent? GetEventWithId(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return null;
        return _realm.All<GameEvent>().FirstOrDefault(e => e.Id == objectId);
    }

    public PaginatedList<GameEvent> GetPaginatedEvents(EventOrderType order, bool descending, EventFilters filters,
        int from, int count, GameUser? accessor)
    {
        return new PaginatedList<GameEvent>(_realm.All<GameEvent>().FilterEvents(this, filters, accessor)
            .OrderEvents(order, descending), from, count);
    }
}