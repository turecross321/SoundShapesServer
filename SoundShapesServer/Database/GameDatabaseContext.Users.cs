using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Requests.Api;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DbUser? GetUserWithName(string name)
    {
        return Users.FirstOrDefault(u => u.Name == name);
    }
    
    public DbUser? GetUserWithEmail(string name)
    {
        return Users.FirstOrDefault(u => u.EmailAddress == name);
    }

    public DbUser CreateUser(string name)
    {
        EntityEntry<DbUser> user = Users.Add(new DbUser
        {
            Name = name,
            Role = UserRole.Default,
            FinishedRegistration = false,
            VerifiedEmail = false,
            CreationDate = Time.Now
        });

        SaveChanges();
        
        // Reload to load the ID
        user.Reload();

        return user.Entity;
    }
    
    public DbUser CreateRegisteredUser(string name, string email, UserRole role)
    {
        EntityEntry<DbUser> user = Users.Add(new DbUser
        {
            Name = name,
            Role = role,
            FinishedRegistration = true,
            VerifiedEmail = true,
            CreationDate = Time.Now,
            EmailAddress = email
        });

        SaveChanges();
        
        // Reload to load the ID
        user.Reload();

        return user.Entity;
    }

    public DbUser SetUserEmail(DbUser user, string email)
    {
        user.EmailAddress = email;
        user.VerifiedEmail = false;
        SaveChanges();
        return user;
    }

    public DbUser VerifyEmail(DbUser user)
    {
        user.VerifiedEmail = true;
        SaveChanges();
        return user;
    }

    public DbUser SetUserPassword(DbUser user, string passwordBcrypt)
    {
        user.PasswordBcrypt = passwordBcrypt;
        SaveChanges();
        return user;
    }

    public DbUser FinishUserRegistration(DbUser user)
    {
        user.FinishedRegistration = true;
        SaveChanges();
        return user;
    }
    
    public DbUser SetUserAuthorizationSettings(DbUser user, ApiAuthorizationSettingsRequest body)
    {
        user.RpcnAuthorization = body.RpcnAuthorization;
        user.PsnAuthorization = body.PsnAuthorization;
        user.IpAuthorization = body.IpAuthorization;
        SaveChanges();
        return user;
    }
}