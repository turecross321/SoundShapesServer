namespace SoundShapesServer.Types;

public enum CodeType
{
    /// <summary>
    /// Code given through the game when logging in for first time. This is used to initialize the registration.
    /// </summary>
    Registration = 0,
    VerifyEmail = 1,
    SetPassword = 2
}