using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Documentation.Errors;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public CommunityTab? GetCommunityTabWithId(string id)
    {
        return _realm.All<CommunityTab>().FirstOrDefault(t => t.Id == id);
    }

    public CommunityTab? CreateCommunityTab(ApiCreateCommunityTabRequest request, GameUser user)
    {
        // More than 4 community tabs will cause performance issues
        if (_realm.All<CommunityTab>().Count() >= 4) return null;
        
        CommunityTab communityTab = new()
        {
            Id = GenerateGuid(),
            ContentType = request.ContentType,
            Title = request.Title,
            Description = request.Description,
            ButtonLabel = request.ButtonLabel,
            Query = request.Query,
            Author = user,
            CreationDate = DateTimeOffset.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow
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
    
    public Response UploadCommunityTabResource(IDataStore dataStore, CommunityTab communityTab, byte[] file)
    {
        if (!IsByteArrayPng(file)) return new Response(BadRequestError.FileIsNotPngWhen, ContentType.Plaintext, HttpStatusCode.BadRequest);

        string key = GetCommunityTabResourceKey(communityTab.Id);
        dataStore.WriteToStore(key, file);

        _realm.Write(() =>
        {
            communityTab.ThumbnailFilePath = key;
        });

        return HttpStatusCode.Created;
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