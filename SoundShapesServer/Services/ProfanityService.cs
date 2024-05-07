using Bunkum.Core.Services;
using NotEnoughLogs;
using Filter = ProfanityFilter.ProfanityFilter;

namespace SoundShapesServer.Services;

public class ProfanityService(Logger logger) : EndpointService(logger)
{
    private readonly Filter _profanityFilter = new();

    public bool ContainsProfanity(string input)
    {
        return _profanityFilter.ContainsProfanity(input);
    }

    public string CensorProfanity(string input, char censorCharacter = '*')
    {
        return _profanityFilter.CensorString(input, censorCharacter);
    }
}