namespace SoundShapesServer.Responses.Api.Framework.Errors;

public class ApiBadRequestError : ApiError
{
    public const string FileIsNotPngWhen = "File is not a PNG.";
    public static readonly ApiBadRequestError FileIsNotPng = new(FileIsNotPngWhen);
    public const string CorruptLevelWhen = "Could not read level. Level is potentially corrupt.";
    public static readonly ApiBadRequestError CorruptLevel = new(CorruptLevelWhen);
    public const string InvalidUsernameWhen = "Requested username is invalid.";
    public static readonly ApiBadRequestError InvalidUsername = new(InvalidUsernameWhen);
    public const string InvalidEmailWhen = "Requested email is invalid.";
    public static readonly ApiBadRequestError InvalidEmail = new(InvalidEmailWhen);
    public const string PasswordIsNotHashedWhen = "Password is not SHA512. Please hash the password.";
    public static readonly ApiBadRequestError PasswordIsNotHashed = new(PasswordIsNotHashedWhen);
    public const string InvalidContentTypeWhen = "Invalid content type.";
    public static readonly ApiBadRequestError InvalidContentType = new(InvalidContentTypeWhen);

    public ApiBadRequestError(string message) : base(message)
    {
    }
}