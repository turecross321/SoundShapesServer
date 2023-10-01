using Bunkum.Core;
using SoundShapesServer;

BunkumConsole.AllocateConsole();

GameServer gameServer = new();
gameServer.Initialize();
gameServer.SetUpAdminUser();
gameServer.ImportLevels();
gameServer.AddOfflineLevels();

gameServer.Start();
await Task.Delay(-1);