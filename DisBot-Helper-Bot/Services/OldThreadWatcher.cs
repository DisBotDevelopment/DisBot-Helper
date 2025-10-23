using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;

namespace DisBot_Helper_Bot.Services;

public class OldThreadWatcher : BackgroundService
{
    private readonly ConfigService ConfigService;
    private readonly RestClient Client;
    private Timer? Timer = null;
    private readonly ILogger Logger;

    public OldThreadWatcher(ConfigService configService, RestClient client, ILogger<OldThreadWatcher> logger)
    {
        ConfigService = configService;
        Client = client;
        Logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Timer = new Timer(CheckThreadMessages, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private async void CheckThreadMessages(object? state)
    {
        try
        {
            var channel =
                await Client.GetActiveGuildThreadsAsync(1084507523492626522);

            foreach (var guildThread in channel)
            {
                if (guildThread.ParentId != ConfigService.Get().SupportHelper.SupportChannelId) continue;
                if ((guildThread as PublicGuildThread)!.AppliedTags!.Contains(ConfigService.Get().SupportHelper
                        .SupportAutoTagId)) return;

                var messages = guildThread.GetMessagesAsync().ToBlockingEnumerable().ToList();
                var lastMessage = messages.First();

                if (!((DateTimeOffset.Now - lastMessage.CreatedAt).TotalMicroseconds > 2)) continue;
                if (lastMessage.Components.Count != 0 && lastMessage.Components.First() is ComponentContainer)
                {
                    if (lastMessage.Components.OfType<ComponentContainer>().First().Components
                        .OfType<TextDisplay>().First()
                        .Content.Contains("1430573811539120349")) return;
                }

                await guildThread.SendMessageAsync(
                    new MessageProperties()
                    {
                        Flags = MessageFlags.IsComponentsV2,
                        Components =
                        [
                            new ComponentContainerProperties()
                            {
                                new TextDisplayProperties(
                                    $"-# <:reply:1430577881205182555> Hey <@{guildThread.OwnerId}> your Thread is older then 10 days it is Active?\n-# If not please use the </resolve:1430573811539120349> command to close it.")
                            }
                        ]
                    }
                );
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
        }
    }
}