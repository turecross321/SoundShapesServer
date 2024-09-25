using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Requests.Api;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DbUser? GetUserWithName(string name)
    {
        return this.Users.FirstOrDefault(u => u.Name == name);
    }
    
    public DbUser? GetUserWithEmail(string name)
    {
        return this.Users.FirstOrDefault(u => u.EmailAddress == name);
    }

    public DbUser CreateUser(string name)
    {
        EntityEntry<DbUser> user = this.Users.Add(new DbUser
        {
            Name = name,
            Role = UserRole.Default,
            FinishedRegistration = false,
            VerifiedEmail = false,
            CreationDate = this.Time.Now,
            RegistrationExpiryDate = this.Time.Now.AddHours(ExpiryTimes.UserRegistrationHours),
        });
        
        this.SaveChanges();
        
        // Reload to load the ID
        user.Reload();

        return user.Entity;
    }
    
    public DbUser CreateRegisteredUser(string name, string email, UserRole role)
    {
        EntityEntry<DbUser> user = this.Users.Add(new DbUser
        {
            Name = name,
            Role = role,
            FinishedRegistration = true,
            VerifiedEmail = true,
            CreationDate = this.Time.Now,
            EmailAddress = email,
        });
        
        this.SaveChanges();
        
        // Reload to load the ID
        user.Reload();

        return user.Entity;
    }

    public DbUser SetUserEmail(DbUser user, string email)
    {
        user.EmailAddress = email;
        user.VerifiedEmail = false;
        this.SaveChanges();
        return user;
    }

    public DbUser VerifyEmail(DbUser user)
    {
        user.VerifiedEmail = true;
        this.SaveChanges();
        return user;
    }

    public DbUser SetUserPassword(DbUser user, string passwordBcrypt)
    {
        user.PasswordBcrypt = passwordBcrypt;
        this.SaveChanges();
        return user;
    }

    public DbUser FinishUserRegistration(DbUser user)
    {
        user.FinishedRegistration = true;
        user.RegistrationExpiryDate = null;
        this.SaveChanges();
        return user;
    }
    
    public DbUser UpdateUserRegistrationExpiryDate(DbUser user)
    {
        user.RegistrationExpiryDate = this._time.Now.AddHours(ExpiryTimes.UserRegistrationHours);
        this.SaveChanges();
        return user;
    }
    
    public DbUser SetUserAuthorizationSettings(DbUser user, ApiAuthorizationSettingsRequest body)
    {
        user.RpcnAuthorization = body.RpcnAuthorization;
        user.PsnAuthorization = body.PsnAuthorization;
        user.IpAuthorization = body.IpAuthorization;
        this.SaveChanges();
        return user;
    }

    public IQueryable<DbUser> GetUsers()
    {
        return this.Users;
    }
}