using CommandLine;

namespace SoundShapesServer;

internal class CommandLineManager
{
    private readonly GameServer _server;

    internal CommandLineManager(GameServer server)
    {
        _server = server;
    }

    internal void StartWithArgs(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args).WithParsed(StartWithOptions);
    }

    private void StartWithOptions(Options options)
    {
        if (options.CommunityLevelsImportPath != null)
            _server.ImportCommunityLevels(options.CommunityLevelsImportPath, options.ImportOverwrite);

        if (options.CampaignLevelImportPath != null)
            _server.ImportCampaignLevels(options.CampaignLevelImportPath, options.ImportOverwrite);
    }

    [Serializable]
    private class Options
    {
        [Option('i', "import_community_levels", Required = false,
            HelpText =
                "Specify path to import server data levels from, such as the following: https://archive.org/details/SoundShapesServersideDataArchive")]
        public string? CommunityLevelsImportPath { get; set; }

        [Option('I', "import_campaign_levels", Required = false,
            HelpText = "Specify path to extracted psarc folder to import campaign levels from")]
        public string? CampaignLevelImportPath { get; set; }

        [Option('o', "import_overwrite", Required = false,
            HelpText = "Decide if already imported levels should be overwritten during import")]
        public bool? ImportOverwrite { get; set; }
    }
}