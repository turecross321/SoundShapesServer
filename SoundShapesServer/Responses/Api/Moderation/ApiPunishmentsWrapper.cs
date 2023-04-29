namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiPunishmentsWrapper
{
    public ApiPunishmentResponse[] Punishments { get; set; }
    public int Count { get; set; }
}