namespace DisBot_Helper_Bot.Config;

public class AppConfig
{
    public string DiscordBotToken { get; set; }
    public ulong UpdateInfoPingRoleId { get; set; } = 1163773397096988683;
    public string GitHubAuthToken { get; set; }
    public SupportHelper SupportHelper { get; set; } = new SupportHelper();
    public StatusHelper StatusHelper { get; set; } = new StatusHelper();
    public SuggestionHelper SuggestionHelper { get; set; } = new SuggestionHelper();
}

public class StatusHelper
{
    public ulong StatusRoleId { get; set; } = 1163773348719890473;
}

public class SuggestionHelper
{
    public ulong SuggestionChannelId { get; set; } = 1210544192145854505;
    public ulong SuggestionImplementedTagId { get; set; } = 1430126584819482725;
    public ulong SuggestionInDevelopmentTagId { get; set; } = 1430126632202666047;
    public ulong SuggestionRejectedTagId { get; set; } = 1430128043199959102;
}

public class SupportHelper
{
    public ulong SupportResolveTagId { get; set; } = 1342822457676922901;
    public ulong SupportAutoTagId { get; set; } = 1366430599811694622;
    public ulong SupportChannelId { get; set; } = 1210544354167627786;

    public string DocsAPIUrl { get; set; } = "https://doc.xyzhub.link/api";
    public string DocsAPIToken { get; set; } = "";
    public string DocsId { get; set; } = "463df892-651f-4f55-a43d-ad722dd580ac";
}