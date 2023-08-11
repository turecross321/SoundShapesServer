namespace SoundShapesServer.Documentation.Errors;

public class BadRequestError
{
    public const string FileIsNotPngWhen = "File is not a PNG.";
    public const string CorruptLevelWhen = "Could not read level. Level is potentially corrupt.";
    public const string InvalidUsernameWhen = "Requested username is invalid.";
    public const string InvalidEmailWhen = "Requested email is invalid.";
    public const string PasswordIsNotHashedWhen = "Password is not SHA512. Please hash the password.";
}