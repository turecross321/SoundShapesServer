using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public Service CreateService(string display_name)
    {
        Service service = new()
        {
            display_name = display_name
        };
        
        this._realm.Write(() =>
        {
            this._realm.Add(service);
        });

        return service;
    }
    
    public Service? GetServiceWithDisplayName(string? display_name)
    {
        if (display_name == null) return null;
        return this._realm.All<Service>().FirstOrDefault(u => u.display_name == display_name);
    }
    
    public Service? GetServiceWithId(string? id)
    {
        if (id == null) return null;
        return this._realm.All<Service>().FirstOrDefault(u => u.id == id);
    }
}