using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public Service CreateService(string displayName)
    {
        Service service = new()
        {
            DisplayName = displayName
        };
        
        this._realm.Write(() =>
        {
            this._realm.Add(service);
        });

        return service;
    }
    
    public Service? GetServiceWithDisplayName(string? displayName)
    {
        if (displayName == null) return null;
        return this._realm.All<Service>().FirstOrDefault(u => u.DisplayName == displayName);
    }
    
    public Service? GetServiceWithId(string? id)
    {
        if (id == null) return null;
        return this._realm.All<Service>().FirstOrDefault(u => u.Id == id);
    }
}