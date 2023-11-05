using Bunkum.Core;
using SoundShapesServer;

BunkumConsole.AllocateConsole();

GameServer gameServer = new();
gameServer.Initialize();
gameServer.SetUpAdminUser();

if (args.Length > 0)
{
    CommandLineManager cli = new(gameServer);
    cli.StartWithArgs(args);
    return;
}

gameServer.Start();
await Task.Delay(-1);