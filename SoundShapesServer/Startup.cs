using Bunkum.HttpServer;
using SoundShapesServer;

BunkumConsole.AllocateConsole();

GameServer gameServer = new();
gameServer.Initialize();
gameServer.SetUpAdminUser();
gameServer.ImportLevels();

gameServer.Start();
await Task.Delay(-1);