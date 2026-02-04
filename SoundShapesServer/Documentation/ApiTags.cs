using NSwag;

namespace SoundShapesServer.Documentation;

public static class ApiTags
{
    public const string Authentication = "Authentication";
    public const string AuthorizationSettings = "Authorization Settings";
    public const string InstanceInformation = "Instance Information";
    public const string Users = "Users";
    
    public static readonly OpenApiTag[] Tags = [
        new OpenApiTag {Name = Authentication, Description = ""},
        new OpenApiTag {Name = AuthorizationSettings, Description = ""},
        new OpenApiTag {Name = InstanceInformation, Description = ""},
        new OpenApiTag {Name = Users, Description = ""},
    ];
}
