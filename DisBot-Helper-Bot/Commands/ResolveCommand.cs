using DisBot_Helper_Bot.Services;
using DisBot_Helper_Bot.Templates;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace DisBot_Helper_Bot.Commands;

public class ResolveCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly ConfigService ConfigService;

    public ResolveCommand(ConfigService configService)
    {
        ConfigService = configService;
    }

    [SlashCommand("resolve", "Resolve your Support Request!")]
    public async Task Resolve()
    {
        if ((Context.Channel as TextGuildChannel)?.ParentId == ConfigService.Get().SupportHelper.SupportChannelId)
        {
            var forumThread = await ((Context.Channel as PublicGuildThread)!).GetAsync();

            if (forumThread.AppliedTags!.Contains(ConfigService.Get().SupportHelper.SupportResolveTagId))
            {
                await Context.Interaction.SendResponseAsync(
                    InteractionCallback.Message(new InteractionMessageProperties()
                    {
                        Flags = MessageFlags.Ephemeral,
                        Content = "-# <:reply:1430577881205182555> You cant resolve it twice!"
                    })
                );
                await forumThread.ModifyAsync(options => { options.Archived = true; });
                return;
            }

            await Context.Interaction.SendResponseAsync(InteractionCallback.Message(
                new InteractionMessageProperties()
                {
                    Flags = MessageFlags.IsComponentsV2,
                    Components =
                    [
                        new ComponentContainerProperties([
                            new TextDisplayProperties(
                                "-# <:reply:1430577881205182555> **<a:1412907566308135012:1430128825865474111> Your Post has been marked as resolved thanks for the request.**"
                            ),
                            new ActionRowProperties()
                            {
                                new ButtonProperties("unResolveReopen", label: "Re-Open",
                                    EmojiProperties.Custom(1289668008503148649), ButtonStyle.Secondary)
                            }
                        ])
                    ]
                }));

            var forumTags = new List<ulong>()
            {
                ConfigService.Get().SupportHelper.SupportResolveTagId
            };
            if (forumThread.AppliedTags != null)
                foreach (var forumThreadAppliedTag in forumThread.AppliedTags)
                {
                    forumTags.Add(forumThreadAppliedTag);
                }

            await forumThread.ModifyAsync(options =>
            {
                options.Archived = true;
                options.AddAppliedTags(forumTags);
            });
        }
        else
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.Message(Messages.ErrorMessage));
        }
    }
}