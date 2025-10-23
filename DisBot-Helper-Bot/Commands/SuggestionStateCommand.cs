using DisBot_Helper_Bot.Enums;
using DisBot_Helper_Bot.Services;
using DisBot_Helper_Bot.Templates;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace DisBot_Helper_Bot.Commands;

public class SuggestionStateCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly ConfigService ConfigService;
    private readonly SuggestionStateService SuggestionStateService;

    public SuggestionStateCommand(ConfigService configService, SuggestionStateService suggestionStateService)
    {
        ConfigService = configService;
        SuggestionStateService = suggestionStateService;
    }

    [SlashCommand("suggestion-state", "Update the suggestion state",
        DefaultGuildPermissions = Permissions.MentionEveryone,
        Contexts = [InteractionContextType.Guild]
    )]
    public async Task Message(
        [SlashCommandParameter(Name = "state", Description = "State of the Suggestion")]
        SuggestionState state,
        [SlashCommandParameter(Name = "message", Description = "Extra Message")]
        string message,
        [SlashCommandParameter(Name = "user", Description = "Users to mention")]
        User? mention = null) 
    {
        if ((Context.Channel as TextGuildChannel)?.ParentId == ConfigService.Get().SuggestionHelper.SuggestionChannelId)
        {
            var forumThread = await ((Context.Channel as PublicGuildThread)!).GetAsync();

            await Context.Interaction.SendResponseAsync(InteractionCallback.Message(
                new InteractionMessageProperties()
                {
                    Flags = MessageFlags.IsComponentsV2,
                    Components =
                    [
                        new ComponentContainerProperties([
                            new TextDisplayProperties(
                                $"{(mention != null ? $"### Hey {mention} you suggestion got Updates!" : "")}\n\n-# {SuggestionStateService.GetState(state)} {message}")
                        ])
                        {
                            AccentColor = state switch
                            {
                                SuggestionState.InDevelopment => new Color(0, 0, 255),
                                SuggestionState.Implemented => new Color(0, 255, 0),
                                SuggestionState.Rejected => new Color(255, 0, 0),
                                _ => new Color(66, 69, 73)
                            }
                        }
                    ]
                }));

            await forumThread.ModifyAsync(options =>
            {
                options.AddAppliedTags(
                    SuggestionStateService.SetSuggestionTags(state, forumThread.AppliedTags!.ToArray()));
            });
        }
        else
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.Message(Messages.ErrorMessage));
        }
    }
}