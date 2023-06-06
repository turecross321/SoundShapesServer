// ReSharper disable InconsistentNaming
namespace SoundShapesServer.Types;

public enum GameContentType
{
    User = 0,
    Level = 1,
    Event = 2,
    Album = 3,
    RemovedLevelAuthor = 4, // Type that is assigned to the Author UserResponse when the author's level has been removed 
    Like = 5,
    Queued = 6,
    Follow = 7,
    Link = 8,
    Version = 9,
    PublishedLevel = 10 // Game gets when it just published a level
}