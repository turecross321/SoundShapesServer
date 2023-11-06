using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Core.Storage;
using Bunkum.Listener.Protocol;
using Bunkum.ProfanityFilter;
using HttpMultipartParser;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Levels;

// ReSharper disable once ClassNeverInstantiated.Global
public class LevelManagementEndpoints : EndpointGroup
{
    // Gets called by Endpoints.cs
    public static Response CreateLevel(RequestContext context, GameServerConfig config, IDataStore dataStore,
        ProfanityService profanity, MultipartFormDataParser parser, GameDatabaseContext database, GameUser user,
        GameToken token)
    {
        if (user.Levels.Count() >= config.LevelPublishLimit)
            return HttpStatusCode.Forbidden;

        string name = parser.GetParameterValue("title");
        int language = int.Parse(parser.GetParameterValue("sce_np_language"));

        DateTimeOffset now = DateTimeOffset.UtcNow;
        GameLevel level = new()
        {
            Id = IdHelper.GenerateLevelId(),
            Author = user,
            Name = profanity.CensorSentence(name),
            Language = language,
            Visibility = LevelVisibility.Public,
            UploadPlatform = token.PlatformType,
            CreationDate = now,
            ModificationDate = now
        };

        database.AddLevel(level, true);

        Response uploadedResources = UploadLevelResources(context, database, dataStore, parser, level, user);
        if (uploadedResources.StatusCode != HttpStatusCode.Created)
            return uploadedResources;

        return new Response(new LevelPublishResponse(level), ContentType.Json, HttpStatusCode.Created);
    }

    // Gets called by Endpoints.cs
    public static Response UpdateLevel(RequestContext context, IDataStore dataStore, ProfanityService profanity,
        MultipartFormDataParser parser, GameDatabaseContext database, GameUser user, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);

        if (level == null) return HttpStatusCode.NotFound;
        if (level.Author.Id != user.Id) return HttpStatusCode.Forbidden;

        string name = parser.GetParameterValue("title");
        int language = int.Parse(parser.GetParameterValue("sce_np_language"));

        level = database.EditLevel(level, name, language);
        
        Response uploadedResources = UploadLevelResources(context, database, dataStore, parser, level, user);
        if (uploadedResources.StatusCode != HttpStatusCode.Created)
            return uploadedResources;

        return new Response(new LevelPublishResponse(level), ContentType.Json, HttpStatusCode.Created);
    }


    private static Response UploadLevelResources(RequestContext context, GameDatabaseContext database,
        IDataStore dataStore,
        IMultipartFormDataParser parser, GameLevel level, GameUser user)
    {
        byte[]? levelFile = null;
        byte[]? thumbnailFile = null;
        byte[]? soundFile = null;

        foreach (FilePart? file in parser.Files)
        {
            byte[] bytes = file.ToByteArray();
            FileType fileType = file.SSFileType();

            switch (fileType)
            {
                case FileType.Level:
                    levelFile = bytes;
                    break;
                case FileType.Image:
                    thumbnailFile = bytes;
                    break;
                case FileType.Sound:
                    soundFile = bytes;
                    break;
                case FileType.Unknown:
                default:
                    context.Logger.LogInfo(BunkumCategory.Filter,
                        user.Username + " attempted to upload an illegal file: " + file.ContentType);
                    return HttpStatusCode.BadRequest;
            }
        }

        if (levelFile == null || thumbnailFile == null || soundFile == null)
        {
            context.Logger.LogInfo(BunkumCategory.Filter, user.Username + " did not upload all the required files.");
            return HttpStatusCode.BadRequest;
        }
        
        
        database.UploadLevelResource(dataStore, level, levelFile, FileType.Level);
        database.UploadLevelResource(dataStore, level, thumbnailFile, FileType.Image);
        database.UploadLevelResource(dataStore, level, soundFile, FileType.Sound);

        return HttpStatusCode.Created;
    }

    // Gets called by Endpoints.cs
    public static Response RemoveLevel(IDataStore dataStore, GameDatabaseContext database, GameUser user,
        GameLevel level)
    {
        if (level.Author.Id != user.Id) return new Response(HttpStatusCode.Forbidden);

        database.RemoveLevel(level, dataStore);

        return HttpStatusCode.OK;
    }
}