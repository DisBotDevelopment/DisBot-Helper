using DisBot_Helper_Bot.Enums;

namespace DisBot_Helper_Bot.Services;

public class SuggestionStateService
{
    private readonly ConfigService ConfigService;

    public SuggestionStateService(ConfigService configService)
    {
        ConfigService = configService;
    }

    public string GetState(SuggestionState state)
    {
        return state switch
        {
            SuggestionState.Implemented => "<a:1412907566308135012:1430128825865474111>",
            SuggestionState.Rejected => "<a:1412907566308135012:1430128825865474111>",
            SuggestionState.InDevelopment => "<:discordbotdev:1147235048702099476>",
            _ => "N/A"
        };
    }

    private ulong GetTagFromState(SuggestionState state)
    {
        return state switch
        {
            SuggestionState.Implemented => ConfigService.Get().SuggestionHelper.SuggestionImplementedTagId,
            SuggestionState.Rejected
                => ConfigService.Get().SuggestionHelper.SuggestionRejectedTagId,
            SuggestionState.InDevelopment => ConfigService.Get().SuggestionHelper.SuggestionInDevelopmentTagId,
            _ => ConfigService.Get().SuggestionHelper.SuggestionRejectedTagId,
        };
    }

    public ulong[] SetSuggestionTags(SuggestionState state, ulong[] tags)
    {
        var forumTags = tags.Where(tag =>
            !(tag == ConfigService.Get().SuggestionHelper.SuggestionImplementedTagId ||
              tag == ConfigService.Get().SuggestionHelper.SuggestionRejectedTagId ||
              tag == ConfigService.Get().SuggestionHelper.SuggestionInDevelopmentTagId)).ToList();

        forumTags.Add(GetTagFromState(state));
        return forumTags.ToArray();
    }
}