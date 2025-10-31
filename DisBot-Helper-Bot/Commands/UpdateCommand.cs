using System.Reflection;
using DisBot_Helper_Bot.Services;
using DisBot_Helper_Bot.Templates;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace DisBot_Helper_Bot.Commands;

public class UpdateCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly ConfigService ConfigService;

    public UpdateCommand(ConfigService configService)
    {
        ConfigService = configService;
    }

    [SlashCommand("update-message", "Send a Update message in the Channel",
        DefaultGuildPermissions = Permissions.MentionEveryone,
        Contexts = [InteractionContextType.Guild]
    )]
    public async Task Message(
        [SlashCommandParameter(Name = "version", Description = "Version String from DisBot")]
        string version,
        [SlashCommandParameter(Name = "changelog-url", Description = "Changelog url to github or docs.")]
        string? changelogUrl = null)
    {
        await Context.Interaction.SendResponseAsync(
            InteractionCallback.Message(new InteractionMessageProperties()
            {
                Flags = MessageFlags.IsComponentsV2,
                Components =
                [
                    new ComponentContainerProperties()
                    {
                        new TextDisplayProperties(
                            $"### <a:1412907566308135012:1430128825865474111> <@1063079377975377960> has been updated to `{version}`! {(string.IsNullOrEmpty(changelogUrl) ? "" : $"Read the [Changelog]({changelogUrl}).")}\n-# <@&{ConfigService.Get().UpdateInfoPingRoleId}>")
                    }
                ]
            })
        );
    }
}