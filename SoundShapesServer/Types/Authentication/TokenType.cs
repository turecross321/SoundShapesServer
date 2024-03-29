namespace SoundShapesServer.Types.Authentication;
public enum TokenType
{
    GameAccess = 0,
    ApiAccess = 1,
    ApiRefresh = 2,
    SetPassword = 3,
    SetEmail = 4,
    AccountDeletion = 5,
    GameUnAuthorized = 6,
    AccountRegistration = 7
}