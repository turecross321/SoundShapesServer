using Newtonsoft.Json;
using SoundShapesServer.Responses.Game.Albums;
using SoundShapesServer.Responses.Game.Albums.LevelInfo;
using SoundShapesServer.Responses.Game.Albums.Levels;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class AlbumHelper
{
    public static AlbumsWrapper AlbumsToAlbumsWrapper(string sessionId, IQueryable<GameAlbum> albums, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(albums.Count(), from, count);
        GameAlbum[] paginatedAlbums = PaginationHelper.PaginateAlbums(albums, from, count);

        List<AlbumResponse> albumResponses = new ();

        for (int i = 0; i < paginatedAlbums.Length; i++)
        {
            AlbumResponse? albumResponse = AlbumToAlbumResponse(paginatedAlbums[i], sessionId);
            if (albumResponse != null) albumResponses.Add(albumResponse);
        }
        
        return new AlbumsWrapper()
        {
            Albums = albumResponses.ToArray(),
            PreviousToken = previousToken,
            NextToken = nextToken
        };
    }

    private static AlbumResponse? AlbumToAlbumResponse(GameAlbum? album, string sessionId)
    {
        if (album == null) return null;
        
        LinerNoteResponse[] linerNoteResponses = LinerNotesToLinerNoteResponses(album.LinerNotes);
        LinerNotesWrapper linerNoteWrapper = LinerNoteResponsesToLinerNoteResponseWrapper(linerNoteResponses);
        string linerNotesString = JsonConvert.SerializeObject(linerNoteWrapper);

        return new AlbumResponse()
        {
            Id = album.Id,
            Type = ResponseType.link.ToString(),
            CreationDate = album.CreationDate.ToUnixTimeMilliseconds().ToString(),
            Target = new AlbumTarget
            {
                Id = IdFormatter.FormatAlbumId(album.Id),
                Type = ResponseType.album.ToString(),
                Metadata = new AlbumMetadata
                {
                    Artist = album.Artist,
                    LinerNotes = linerNotesString,
                    SidePanelUrl = ResourceHelper.GenerateAlbumResourceUrl(album.Id, AlbumResourceType.sidePanel, sessionId),
                    CreationDate = album.CreationDate.ToString(),
                    Name = album.Name,
                    ThumbnailUrl = ResourceHelper.GenerateAlbumResourceUrl(album.Id, AlbumResourceType.thumbnail, sessionId)
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
            Items = levelResponses.ToArray(),
            PreviousToken = previousToken,
            NextToken = nextToken
        };
    }

    private static AlbumLevelInfoResponse? LevelToAlbumLevelInfoResponse(GameUser user, GameAlbum? album, GameLevel? level)
    {
        if (album == null || level == null) return null;

        return new AlbumLevelInfoResponse()
        {
            Id = IdFormatter.FormatAlbumLinkId(album.Id, level.Id),
            Timestamp = level.ModificationDate.ToUnixTimeMilliseconds(),
            Target = new AlbumLevelInfoTarget()
            {
                Id = IdFormatter.FormatLevelId(level.Id),
                Type = ResponseType.level.ToString(),
                Completed = level.UsersWhoHaveCompletedLevel.Contains(user)
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
            Levels = levelResponses.ToArray(),
            PreviousToken = previousToken,
            NextToken = nextToken
        };
    }
    
    private static AlbumLevelResponse? LevelToAlbumLevelResponse(GameUser user, GameAlbum? album, GameLevel? level)
    {
        if (album == null || level == null) return null;
        
        return new AlbumLevelResponse()
        {
            Id = IdFormatter.FormatAlbumLinkId(album.Id, level.Id),
            Type = ResponseType.link.ToString(),
            Timestamp = level.ModificationDate.ToUnixTimeMilliseconds(),
            Target = new AlbumLevelTarget
            {
                Id = IdFormatter.FormatLevelId(level.Id),
                Type = ResponseType.level.ToString(),
                Completed = level.UsersWhoHaveCompletedLevel.Contains(user),
                LatestVersion = new LevelVersionResponse
                {
                    Id = IdFormatter.FormatVersionId(level.ModificationDate.ToUnixTimeMilliseconds().ToString()),
                },
                Author = new UserResponse
                {
                    Id = IdFormatter.FormatUserId(level.Author.Id),
                    Metadata = UserHelper.GenerateUserMetadata(level.Author)
                },
                Metadata = LevelHelper.GenerateMetadataResponse(level)
            }
        };
    }

    private static LinerNotesWrapper LinerNoteResponsesToLinerNoteResponseWrapper(LinerNoteResponse[] linerNotes)
    {
        return new LinerNotesWrapper()
        {
            LinerNotes = linerNotes,
            Version = 1
        };
    }
    
    private static LinerNoteResponse[] LinerNotesToLinerNoteResponses(IList<LinerNote> linerNotes)
    {
        List<LinerNoteResponse> responses = new ();

        for (int i = 0; i < linerNotes.Count; i++)
        {
            responses.Add(new LinerNoteResponse
            {
                Text = linerNotes[i].Text,
                FontType = linerNotes[i].FontType
            });
        }

        return responses.ToArray();
    }
}