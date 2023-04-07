using Bunkum.HttpServer.Responses;
using Newtonsoft.Json;
using SoundShapesServer.Enums;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Albums;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class AlbumHelper
{
    public static AlbumResponseWrapper ConvertAlbumArrayToAlbumResponseWrapper(GameAlbum[] albums, int totalEntries, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalEntries, from, count);

        AlbumResponse[] albumResponses = new AlbumResponse[albums.Length];

        for (int i = 0; i < albums.Length; i++)
        {
            albumResponses[i] = ConvertAlbumToAlbumResponse(albums[i]);
        }
        
        return new AlbumResponseWrapper()
        {
            items = albumResponses,
            previousToken = previousToken,
            nextToken = nextToken
        };
    }

    private static AlbumResponse ConvertAlbumToAlbumResponse(GameAlbum album)
    {
        LinerNoteResponse[] linerNoteResponses = LinerNotesToLinerNoteResponses(album.linerNotes);
        LinerNoteResponseWrapper linerNoteWrapper = LinerNoteResponsesToLinerNoteResponseWrapper(linerNoteResponses);
        string linerNotesString = JsonConvert.SerializeObject(linerNoteWrapper);
        
        return new AlbumResponse()
        {
            id = album.id,
            type = ResponseType.link.ToString(),
            timestamp = album.date.ToUnixTimeMilliseconds().ToString(),
            target = new AlbumTarget()
            {
                id = IdFormatter.FormatAlbumId(album.id),
                type = ResponseType.album.ToString(),
                metadata = new AlbumMetadata()
                {
                    albumArtist = album.artist,
                    linerNotes = linerNotesString,
                    sidePanelURL = album.sidePanelURL,
                    date = "Sept 17 2015", // TODO: REMOVE THIS SHIT
                    displayName = album.name,
                    thumbnailURL = album.thumbnailURL
                }
            }
        };
    }

    public static AlbumLevelResponseWrapper LevelsToAlbumLevelResponseWrapper
        (GameUser user, GameAlbum album, GameLevel[] levels, int totalEntries, int from, int count)
    {
        AlbumLevelResponse[] levelResponses = new AlbumLevelResponse[levels.Length];
        
        for (int i = 0; i < levels.Length; i++)
        {
            levelResponses[i] = LevelToAlbumLevelResponse(user, album, levels[i]);
        }
        
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalEntries, from, count);

        return new AlbumLevelResponseWrapper()
        {
            items = levelResponses,
            previousToken = previousToken,
            nextToken = nextToken
        };
    }
    
    private static AlbumLevelResponse LevelToAlbumLevelResponse(GameUser user, GameAlbum album, GameLevel level)
    {
        return new AlbumLevelResponse()
        {
            id = IdFormatter.FormatAlbumLevelId(album.id, level.id),
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

    private static LinerNoteResponseWrapper LinerNoteResponsesToLinerNoteResponseWrapper(LinerNoteResponse[] linerNotes)
    {
        return new LinerNoteResponseWrapper()
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
}