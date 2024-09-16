using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;

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
            Role = UserRole.Default
        });

        SaveChanges();
        
        // Reload to load the ID
        user.Reload();

        return user.Entity;
    }

    public void SetUserEmail(DbUser user, string email)
    {
        user.EmailAddress = email;
        user.VerifiedEmail = false;
        SaveChanges();
    }

    public void VerifyEmail(DbUser user)
    {
        user.VerifiedEmail = true;
        SaveChanges();
    }

    public void SetUserPassword(DbUser user, string passwordBcrypt)
    {
        user.PasswordBcrypt = passwordBcrypt;
        SaveChanges();
    }

    public void FinishUserRegistration(DbUser user)
    {
        user.FinishedRegistration = true;
        SaveChanges();
    }
}