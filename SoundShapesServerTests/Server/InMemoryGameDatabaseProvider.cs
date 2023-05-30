using SoundShapesServer.Database;

namespace SoundShapesServerTests.Server;

public class InMemoryGameDatabaseProvider : GameDatabaseProvider
{
    private readonly int _databaseId = Random.Shared.Next();
    // ReSharper disable once StringLiteralTypo
    protected override string Filename => $"realm-inmemory-{_databaseId}";
    protected override bool InMemory => true;
}