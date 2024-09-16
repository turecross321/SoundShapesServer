namespace SoundShapesServer.Types.Config;

public record InstanceSettings
{
    public string InstanceName { get; set; } = "Sound Shapes Server"; 
    public string Description { get; set; } = "A SoundShapesServer instance";
    public string? BannerUrl { get; set; } = null;

}