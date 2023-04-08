using Newtonsoft.Json;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Albums;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class AlbumHelper
{
    public static AlbumsWrapper AlbumsToAlbumsWrapper(GameAlbum[] albums, int totalEntries, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalEntries, from, count);

        List<AlbumResponse> albumResponses = new ();

        for (int i = 0; i < albums.Length; i++)
        {
            AlbumResponse? albumResponse = AlbumToAlbumResponse(albums[i]);
            if (albumResponse != null) albumResponses.Add(albumResponse);
        }
        
        return new AlbumsWrapper()
        {
            items = albumResponses.ToArray(),
            previousToken = previousToken,
            nextToken = nextToken
        };
    }

    private static AlbumResponse? AlbumToAlbumResponse(GameAlbum? album)
    {
        if (album == null) return null;
        
        LinerNoteResponse[] linerNoteResponses = LinerNotesToLinerNoteResponses(album.linerNotes);
        LinerNotesWrapper linerNoteWrapper = LinerNoteResponsesToLinerNoteResponseWrapper(linerNoteResponses);
        string linerNotesString = JsonConvert.SerializeObject(linerNoteWrapper);
        
        return new AlbumResponse()
        {
            id = album.id,
            type = ResponseType.link.ToString(),
            timestamp = album.date.ToUnixTimeMilliseconds().ToString(),
            target = new AlbumTarget
            {
                id = IdFormatter.FormatAlbumId(album.id),
                type = ResponseType.album.ToString(),
                metadata = new AlbumMetadata
                {
                    albumArtist = album.artist,
                    linerNotes = linerNotesString,
                    sidePanelURL = GenerateAlbumResourceUrl(album.id, AlbumResourceType.sidePanel),
                    date = album.date.ToString(),
                    displayName = album.name,
                    thumbnailURL = GenerateAlbumResourceUrl(album.id, AlbumResourceType.thumbnail)
                }
            }
        };
    }

    public static AlbumLevelInfosWrapper LevelsToAlbumLevelInfosWrapper
        (GameUser user, GameAlbum album, GameLevel[] levels, int totalEntries, int from, int count)
    {
        List<AlbumLevelInfoResponse> levelResponses = new ();
        
        for (int i = 0; i < levels.Length; i++)
        {
            AlbumLevelInfoResponse? levelResponse = LevelToAlbumLevelInfoResponse(user, album, levels[i]);
            if (levelResponse != null) levelResponses.Add(levelResponse);
        }
        
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalEntries, from, count);

        return new AlbumLevelInfosWrapper()
        {
            items = levelResponses.ToArray(),
            previousToken = previousToken,
            nextToken = nextToken
        };
    }

    private static AlbumLevelInfoResponse? LevelToAlbumLevelInfoResponse(GameUser user, GameAlbum? album, GameLevel? level)
    {
        if (album == null || level == null) return null;

        return new AlbumLevelInfoResponse()
        {
            id = IdFormatter.FormatAlbumLinkId(album.id, level.id),
            timestamp = level.modified.ToUnixTimeMilliseconds(),
            target = new AlbumLevelInfoTarget()
            {
                id = IdFormatter.FormatLevelId(level.id),
                type = ResponseType.level.ToString(),
                completed = level.completionists.Contains(user)
            }
        };
    }
    
    public static AlbumLevelsWrapper LevelsToAlbumLevelsWrapper
        (GameUser user, GameAlbum album, GameLevel[] levels, int totalEntries, int from, int count)
    {
        List<AlbumLevelResponse> levelResponses = new ();
        
        for (int i = 0; i < levels.Length; i++)
        {
            AlbumLevelResponse? levelResponse = LevelToAlbumLevelResponse(user, album, levels[i]);
            if (levelResponse != null) levelResponses.Add(levelResponse);
        }
        
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalEntries, from, count);

        return new AlbumLevelsWrapper()
        {
            items = levelResponses.ToArray(),
            previousToken = previousToken,
            nextToken = nextToken
        };
    }
    
    private static AlbumLevelResponse? LevelToAlbumLevelResponse(GameUser user, GameAlbum? album, GameLevel? level)
    {
        if (album == null || level == null) return null;
        
        return new AlbumLevelResponse()
        {
            id = IdFormatter.FormatAlbumLinkId(album.id, level.id),
            type = ResponseType.link.ToString(),
            timestamp = level.modified.ToUnixTimeMilliseconds(),
            target = new AlbumLevelTarget
            {
                id = IdFormatter.FormatLevelId(level.id),
                type = ResponseType.level.ToString(),
                completed = level.completionists.Contains(user),
                latestVersion = new LevelVersionResponse
                {
                    id = IdFormatter.FormatVersionId(level.modified.ToUnixTimeMilliseconds().ToString()),
                },
                author = new UserResponse
                {
                    id = IdFormatter.FormatUserId(level.author.id),
                    type = ResponseType.identity.ToString(),
                    metadata = UserHelper.GenerateUserMetadata(user)
                },
                metadata = LevelHelper.GenerateMetadataResponse(level)
            }
        };
    }

    private static LinerNotesWrapper LinerNoteResponsesToLinerNoteResponseWrapper(LinerNoteResponse[] linerNotes)
    {
        return new LinerNotesWrapper()
        {
            linerNotes = linerNotes,
            version = 1
        };
    }
    
    private static LinerNoteResponse[] LinerNotesToLinerNoteResponses(IList<LinerNote> linerNotes)
    {
        List<LinerNoteResponse> responses = new ();

        for (int i = 0; i < linerNotes.Count; i++)
        {
            responses.Add(new LinerNoteResponse
            {
                text = linerNotes[i].text,
                fontType = linerNotes[i].fontType
            });
        }

        return responses.ToArray();
    }

    private static string GenerateAlbumResourceUrl(string albumId, AlbumResourceType type)
    {
        return $"otg/~album:{albumId}/~content:{type.ToString()}.png/data.get";
    }
}