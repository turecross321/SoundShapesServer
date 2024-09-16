using Bunkum.Core;
using SoundShapesServer;
using SoundShapesServer.Database;

BunkumConsole.AllocateConsole();

using SSServer<GameDatabaseProvider> server = new();

server.Start();
await Task.Delay(-1);