using DisBot_Helper_Bot.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace DisBot_Helper_Bot.Commands;

public class DocsCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly DocsService DocsService;
    private readonly ConfigService ConfigService;

    public DocsCommand(DocsService docsService, ConfigService configService)
    {
        DocsService = docsService;
        ConfigService = configService;
    }

    [SlashCommand("docs", "https://docs.disbot.app",
        DefaultGuildPermissions = Permissions.MentionEveryone,
        Contexts = [InteractionContextType.Guild]
    )]
    public async Task Message(
        [SlashCommandParameter(Name = "search", Description = "Search query for the docs.", MaxLength = 20)]
        string search)
    {
        try
        {
            var callback = await DocsService.GetDocsPostAsync(search, 100, 0);

            if (callback == null)
            {
                await Context.Interaction.SendResponseAsync(InteractionCallback.Message(
                    new InteractionMessageProperties()
                    {
                        Flags = MessageFlags.Ephemeral,
                        Content =
                            $"-# <:reply:1430577881205182555> Please open a <#{ConfigService.Get().SupportHelper.SupportChannelId}> request! Or try again later.",
                    }));
            }

            await Context.Interaction.SendResponseAsync(InteractionCallback.Message(new InteractionMessageProperties()
            {
                Flags = MessageFlags.IsComponentsV2,
                Components =
                [
                    new ComponentContainerProperties()
                    {
                        new TextDisplayProperties(
                            $"-# <:reply:1430577881205182555> **Try to read this Document from your Docs. [Read More](https://doc.xyzhub.link/s/disbot{callback!.data[0].document.url})**")
                    }
                ]
            }));
        }
        catch (Exception ex)
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.Message(
                new InteractionMessageProperties()
                {
                    Flags = MessageFlags.Ephemeral,
                    Content =
                        $"-# <:reply:1430577881205182555> <:error:1366430438444236911> OoO..., I only get an error this is not Helpfully please ping @xyzjesper.",
                }));
        }
    }
}