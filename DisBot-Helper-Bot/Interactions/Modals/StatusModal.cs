using DisBot_Helper_Bot.Enums;
using DisBot_Helper_Bot.Services;
using NetCord;
using NetCord.JsonConverters;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace DisBot_Helper_Bot.Interactions.Buttons;

public class StatusModal : ComponentInteractionModule<ModalInteractionContext>
{
    private readonly ConfigService ConfigService;
    private readonly RestClient RestClient;

    public StatusModal(ConfigService configService, RestClient restClient)
    {
        RestClient = restClient;
        ConfigService = configService;
    }

    [ComponentInteraction("openStatusIncidence")]
    public async Task OpenStatusIncidenceExecute()
    {
        Enum.TryParse(Context.Components.OfType<Label>()
            .Select(l => l.Component)
            .OfType<StringMenu>()
            .Select(i => i.SelectedValues).FirstOrDefault()!.FirstOrDefault(), out StatusType type);

        var message = Context.Components.OfType<Label>()
            .Select(l => l.Component)
            .OfType<TextInput>()
            .Where(properties => properties.CustomId == "message")
            .Select(input => input.Value).FirstOrDefault();

        var title = Context.Components.OfType<Label>()
            .Select(l => l.Component)
            .OfType<TextInput>()
            .Where(properties => properties.CustomId == "title")
            .Select(properties => properties.Value).FirstOrDefault();

        var thread = Context.Components.OfType<Label>()
            .Select(l => l.Component)
            .OfType<ChannelMenu>()
            .Where(properties => properties.CustomId == "thread").Select(menu => menu);

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
                var selectedThread = thread.FirstOrDefault().SelectedValues[0] as GuildThread;
                if (selectedThread == null)
                {
                    await Context.Interaction.SendResponseAsync(InteractionCallback.Message(
                        new InteractionMessageProperties
                        {
                            Content = "You need to give a thread channel.",
                            Flags = MessageFlags.Ephemeral
                        }));
                    await Context.Interaction.DeleteResponseAsync();
                    return;
                }

                var threadMessages = RestClient.GetMessagesAsync(selectedThread.Id).ToBlockingEnumerable().ToList();
                var firstMessage = threadMessages.Last();
                var timestamp = Math.Floor((double)firstMessage.CreatedAt.ToUnixTimeMilliseconds() / 1000);

                await selectedThread.SendMessageAsync(new MessageProperties()
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
                var selectedThread = thread.FirstOrDefault().SelectedValues[0] as GuildThread;
                if (selectedThread == null)
                {
                    await Context.Interaction.SendResponseAsync(InteractionCallback.Message(
                        new InteractionMessageProperties
                        {
                            Content = "You need to give a thread channel.",
                            Flags = MessageFlags.Ephemeral
                        }));
                    await Context.Interaction.DeleteResponseAsync();
                    return;
                }

                await selectedThread.SendMessageAsync(new MessageProperties()
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