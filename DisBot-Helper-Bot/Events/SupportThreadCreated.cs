using DisBot_Helper_Bot.Services;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Rest;

namespace DisBot_Helper_Bot.Events;

public class SupportThreadCreated : IGuildThreadCreateGatewayHandler
{
    private readonly ConfigService ConfigService;
    private readonly RestClient RestClient;

    public SupportThreadCreated(ConfigService configService, RestClient restClient)
    {
        ConfigService = configService;
        RestClient = restClient;
    }

    public async ValueTask HandleAsync(GuildThreadCreateEventArgs arg)
    {
        if (arg.Thread.ParentId != ConfigService.Get().SupportHelper.SupportChannelId) return;

        if (((arg.Thread as PublicGuildThread)!).AppliedTags!.Contains(ConfigService.Get().SupportHelper
                .SupportAutoTagId))
        {
            await arg.Thread.SendMessageAsync(new MessageProperties()
            {
                Flags = MessageFlags.IsComponentsV2,
                Components =
                [
                    new ComponentContainerProperties()
                    {
                        new TextDisplayProperties(
                            "-# <:reply:1430577881205182555> **This is an <a:loading:1199076035354968194> Automation Thread thanks for the Report.**\n\n Noting to do. You can use the </docs:1430668136914616550> to get help. Or wait for an Developer!"),
                        new ActionRowProperties()
                        {
                            new LinkButtonProperties("https://docs.disbot.app", "Read the Docs",
                                EmojiProperties.Custom(1376636202631041046))
                        }
                    }
                ]
            });
        }
        else
        {
            await arg.Thread.SendMessageAsync(new MessageProperties()
            {
                Flags = MessageFlags.IsComponentsV2,
                Components =
                [
                    new ComponentContainerProperties()
                    {
                        new TextDisplayProperties(
                            $"-# <:reply:1430577881205182555> Welcome <@{arg.Thread.OwnerId}> to the DisBot Support. I'm the DisBot Helper. You can use this command </docs:1430668136914616550> to search in the docs. I will not generate AI messages at this point. :D"),
                        new ActionRowProperties()
                        {
                            new LinkButtonProperties("https://docs.disbot.app", "Read the Docs",
                                EmojiProperties.Custom(1376636202631041046))
                        }
                    }
                ]
            });
        }
    }
}