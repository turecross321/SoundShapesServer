using Bunkum.HttpServer;
using SoundShapesServer;

BunkumConsole.AllocateConsole();

Server server = new();
server.Initialize();
server.SetUpAdminUser();
server.ImportLevels();

server.Start();
await Task.Delay(-1);