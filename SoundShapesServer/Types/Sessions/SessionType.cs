namespace SoundShapesServer.Types.Sessions;
public enum SessionType
{
    Game = 0,
    Api = 1,
    SetPassword = 2,
    SetEmail = 3,
    RemoveAccount = 4,
    Unauthorized = 5,
    Banned = 6
}