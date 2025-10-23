using DisBot_Helper_Bot.Enums;
using DisBot_Helper_Bot.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace DisBot_Helper_Bot.Commands;

public class StatusCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly ConfigService ConfigService;
    private readonly RestClient RestClient;

    public StatusCommand(ConfigService configService, RestClient restClient)
    {
        ConfigService = configService;
        RestClient = restClient;
    }

    [SlashCommand("status", "Status Update Message",
        DefaultGuildPermissions = Permissions.MentionEveryone,
        Contexts = [InteractionContextType.Guild]
    )]
    public async Task Message(
        [SlashCommandParameter(Name = "type", Description = "Type of the status update")]
        StatusType type,
        [SlashCommandParameter(Name = "message", Description = "Message for the users")]
        string message,
        [SlashCommandParameter(Name = "title", Description = "Thread Title")]
        string? title,
        [SlashCommandParameter(Name = "thread", Description = "Thread for the users")]
        GuildThread? thread = null
    )
    {
        switch (type)
        {
            case StatusType.New:
            {
                var statusMessage = await Context.Channel.SendMessageAsync(new MessageProperties()
                {
                    Flags = MessageFlags.IsComponentsV2,
                    Components =
                    [
                        new ComponentContainerProperties([
                            new TextDisplayProperties(
                                $"-# <:reply:1430577881205182555> <a:1412907652509335692:1430128828801487000> Incidence.\n\n{message}"
                                    .Replace("{p}", $"<@&{ConfigService.Get().StatusHelper.StatusRoleId}>")
                            )
                        ])
                    ]
                });

                var statusThread = await statusMessage.CreateGuildThreadAsync(
                    new GuildThreadFromMessageProperties(title ?? "Status Updates for current Incidence")
                    {
                        AutoArchiveDuration = ThreadArchiveDuration.ThreeDays
                    });
                await statusThread.SendMessageAsync(new MessageProperties()
                {
                    Content =
                        $"-# <:reply:1430577881205182555> <@&{ConfigService.Get().StatusHelper.StatusRoleId}> A new Incidence has been posted!"
                });
            }
                break;
            case StatusType.Resolved:
            {
                if (thread == null) return;

                var threadMessages = RestClient.GetMessagesAsync(thread.Id).ToBlockingEnumerable().ToList();
                var firstMessage = threadMessages.Last();
                var timestamp = Math.Floor((double)firstMessage.CreatedAt.ToUnixTimeMilliseconds() / 1000);

                await thread.SendMessageAsync(new MessageProperties()
                {
                    Flags = MessageFlags.IsComponentsV2,
                    Components =
                    [
                        new ComponentContainerProperties([
                            new TextDisplayProperties(
                                $"{message} \n\n\n-# **<a:1412907566308135012:1430128825865474111> Incidence has beed resolved after <t:{timestamp}:R>**"
                                    .Replace("{p}", $"<@&{ConfigService.Get().StatusHelper.StatusRoleId}>")
                            )
                        ])
                    ]
                });
            }
                break;
            case StatusType.Update:
            {
                if (thread == null) return;

                await thread.SendMessageAsync(new MessageProperties()
                {
                    Flags = MessageFlags.IsComponentsV2,
                    Components =
                    [
                        new ComponentContainerProperties([
                            new TextDisplayProperties(
                                $"-# <:reply:1430577881205182555> <a:1412907639419043880:1430647258990379008> Incidence Update.\n\n{message}"
                                    .Replace("{p}", $"<@&{ConfigService.Get().StatusHelper.StatusRoleId}>")
                            )
                        ])
                    ]
                });
            }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        await Context.Interaction.SendResponseAsync(InteractionCallback.Message(new InteractionMessageProperties()
        {
            Content = "Done",
            Flags = MessageFlags.Ephemeral
        }));
        await Context.Interaction.DeleteResponseAsync();
    }
}