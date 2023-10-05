namespace SoundShapesServer.Types.Sessions;
public enum SessionType
{
    Game = 0,
    Api = 1,
    ApiRefresh = 2,
    SetPassword = 3,
    SetEmail = 4,
    AccountRemoval = 5,
    GameUnAuthorized = 6
}