namespace SoundShapesServer.Types;

public enum TokenType
{
    /// <summary>
    /// Normal token type without any special restrictions given to game client
    /// </summary>
    GameAccess = 0,
    /// <summary>
    /// Used for restricted purposes that only require being able to reach the eula.
    /// </summary>
    GameEula = 1,
    /// <summary>
    /// Used for API endpoints
    /// </summary>
    ApiAccess = 2
}