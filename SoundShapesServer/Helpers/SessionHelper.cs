using SoundShapesServer.Database;

namespace SoundShapesServer.Helpers;

public static class SessionHelper
{
    private const string EmailSessionIdCharacters = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int EmailSessionIdLength = 8;
        
    public static string GenerateEmailSessionId(RealmDatabaseContext database)
    {
        Random r = new Random();
        string levelId = "";
        for (int i = 0; i < EmailSessionIdLength; i++)
        {
            levelId += EmailSessionIdCharacters[r.Next(EmailSessionIdCharacters.Length - 1)];
        }

        if (database.GetSessionWithSessionId(levelId) == null) return levelId; // Return if Id has not been used before
        return GenerateEmailSessionId(database); // Generate new Id if it already exists   
    }
}