using Bunkum.Core.Storage;
using MongoDB.Bson;
using SoundShapesServer.Extensions;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public CommunityTab? GetCommunityTabWithId(string id)
    {
        ObjectId? objectId = ObjectId.Parse(id);

        return _realm.All<CommunityTab>().FirstOrDefault(t => t.Id == objectId);
    }

    public CommunityTab? CreateCommunityTab(ApiCreateCommunityTabRequest request, GameUser user)
    {
        // More than 4 community tabs will cause performance issues
        if (_realm.All<CommunityTab>().Count() >= 4) return null;
        
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        CommunityTab communityTab = new()
        {
            ContentType = request.ContentType,
            Title = request.Title,
            Description = request.Description,
            ButtonLabel = request.ButtonLabel,
            Query = request.Query,
            Author = user,
            CreationDate = now,
            ModificationDate = now
        };
        
        _realm.Write(() =>
        {
            _realm.Add(communityTab);
        });

        return communityTab;
    }

    public CommunityTab EditCommunityTab(CommunityTab communityTab, ApiCreateCommunityTabRequest request, GameUser user)
    {
        _realm.Write(() =>
        {
            communityTab.ContentType = request.ContentType;
            communityTab.Title = request.Title;
            communityTab.Description = request.Description;
            communityTab.ButtonLabel = request.ButtonLabel;
            communityTab.Query = request.Query;
            communityTab.Author = user;
            communityTab.ModificationDate = DateTimeOffset.UtcNow;
        });

        return communityTab;
    }
    
    public ApiOkResponse UploadCommunityTabResource(IDataStore dataStore, CommunityTab communityTab, byte[] file)
    {
        if (!file.IsPng()) 
            return ApiBadRequestError.FileIsNotPng;

        string key = GetCommunityTabResourceKey(communityTab.Id);
        dataStore.WriteToStore(key, file);

        _realm.Write(() =>
        {
            communityTab.ThumbnailFilePath = key;
        });

        return new ApiOkResponse();
    }
    
    public void RemoveCommunityTab(IDataStore dataStore, CommunityTab communityTab)
    {
        if (communityTab.ThumbnailFilePath != null) dataStore.RemoveFromStore(communityTab.ThumbnailFilePath);

        _realm.Write(() =>
        {
            _realm.Remove(communityTab);
        });
    }
    
    public CommunityTab[] GetCommunityTabs()
    {
        return _realm.All<CommunityTab>().ToArray();
    }
}