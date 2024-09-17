namespace SoundShapesServer.Types.Config;

public record EmailSettings
{
    public string Address { get; set; } = "example@gmail.com";
    public string Password { get; set; } = "yeahMan";
    public string Host { get; set; } = "smtp.gmail.com";
    public int HostPort { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
}