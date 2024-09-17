using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Common.Verification;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DbCode CreateCode(DbUser user, CodeType type)
    {
        // Remove all previous instances of the same CodeType
        IQueryable<DbCode> codes = GetCodesForUser(user, type);
        if (codes.Any())
        {
            Codes.RemoveRange(codes);
            SaveChanges();
        }

        string code = CodeHelper.GenerateCode();
        while (GetCode(code) != null)
        {
            code = CodeHelper.GenerateCode();
        }
        
        EntityEntry<DbCode> newCode = Codes.Add(new DbCode
        {
            Code = code,
            CreationDate = _time.Now,
            ExpiryDate = _time.Now.AddHours(ExpiryTimes.CodeHours),
            UserId = user.Id,
            CodeType = type
        });

        SaveChanges();

        return newCode.Entity;
    }
    
    public DbCode? GetCode(string code)
    {
        return Codes.Include(t => t.User).FirstOrDefault(t => t.Code == code);
    }
    
    public DbCode? GetCode(string code, CodeType type)
    {
        return Codes.Include(t => t.User).FirstOrDefault(t => t.Code == code && t.CodeType == type);
    }
    
    public DbCode? GetCode(DbUser user, CodeType type)
    {
        return Codes.Include(t => t.User).FirstOrDefault(t => t.User == user && t.CodeType == type);
    }


    public void RemoveCode(DbCode code)
    {
        Codes.Remove(code);
        SaveChanges();
    }
    
    private IQueryable<DbCode> GetCodesForUser(DbUser user, CodeType type)
    {
        return Codes.Include(t => t.User)
            .Where(t => t.UserId == user.Id && t.CodeType == type);
    }
}