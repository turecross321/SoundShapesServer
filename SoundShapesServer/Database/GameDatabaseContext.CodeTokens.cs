using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Common.Verification;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DbCodeToken CreateCodeToken(DbUser user, CodeTokenType tokenType)
    {
        // Remove all previous instances of the same CodeTokenType
        IQueryable<DbCodeToken> tokens = GetCodeTokensForUser(user, tokenType);
        if (tokens.Any())
        {
            CodeTokens.RemoveRange(tokens);
            SaveChanges();
        }

        string code = CodeHelper.GenerateCode();
        while (GetCodeTokenWithCode(code) != null)
        {
            code = CodeHelper.GenerateCode();
        }
        
        EntityEntry<DbCodeToken> newToken = CodeTokens.Add(new DbCodeToken
        {
            Code = code,
            CreationDate = Now,
            ExpiryDate = Now.AddHours(ExpiryTimes.CodeTokenHours),
            UserId = user.Id,
            TokenType = tokenType
        });

        SaveChanges();

        return newToken.Entity;
    }
    
    public DbCodeToken? GetCodeTokenWithCode(string code)
    {
        return CodeTokens.Include(t => t.User).FirstOrDefault(t => t.Code == code);
    }
    
    public DbCodeToken? GetCodeTokenWithCode(string code, CodeTokenType type)
    {
        return CodeTokens.Include(t => t.User).FirstOrDefault(t => t.Code == code && t.TokenType == type);
    }

    public void RemoveCodeToken(DbCodeToken token)
    {
        CodeTokens.Remove(token);
        SaveChanges();
    }
    
    private IQueryable<DbCodeToken> GetCodeTokensForUser(DbUser user, CodeTokenType type)
    {
        return CodeTokens.Include(t => t.User)
            .Where(t => t.UserId == user.Id && t.TokenType == type);
    }
}