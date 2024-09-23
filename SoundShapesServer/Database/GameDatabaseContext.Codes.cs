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
        IQueryable<DbCode> codes = this.GetCodesForUser(user, type);
        if (codes.Any())
        {
            this.Codes.RemoveRange(codes);
            this.SaveChanges();
        }

        string code = CodeHelper.GenerateCode();
        while (this.GetCode(code) != null)
        {
            code = CodeHelper.GenerateCode();
        }
        
        EntityEntry<DbCode> newCode = this.Codes.Add(new DbCode
        {
            Code = code,
            CreationDate = this._time.Now,
            ExpiryDate = this._time.Now.AddHours(ExpiryTimes.CodeHours),
            UserId = user.Id,
            CodeType = type,
        });
        
        this.SaveChanges();

        return newCode.Entity;
    }
    
    public DbCode? GetCode(string code)
    {
        return this.Codes.Include(t => t.User).FirstOrDefault(t => t.Code == code);
    }
    
    public DbCode? GetCode(string code, CodeType type)
    {
        return this.Codes.Include(t => t.User).FirstOrDefault(t => t.Code == code && t.CodeType == type);
    }
    
    public DbCode? GetCode(DbUser user, CodeType type)
    {
        return this.Codes.Include(t => t.User).FirstOrDefault(t => t.User == user && t.CodeType == type);
    }
    
    
    public IQueryable<DbCode> GetCodes() => this.Codes;
    
    public void RemoveCode(DbCode code)
    {
        this.Codes.Remove(code);
        this.SaveChanges();
    }
    
    private IQueryable<DbCode> GetCodesForUser(DbUser user, CodeType type)
    {
        return this.Codes.Include(t => t.User)
            .Where(t => t.UserId == user.Id && t.CodeType == type);
    }
}