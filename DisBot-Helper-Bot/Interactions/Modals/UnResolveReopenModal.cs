using DisBot_Helper_Bot.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace DisBot_Helper_Bot.Interactions.Buttons;

public class UnResolveReopenModal : ComponentInteractionModule<ModalInteractionContext>
{
    private readonly ConfigService ConfigService;

    public UnResolveReopenModal(ConfigService configService)
    {
        ConfigService = configService;
    }

    [ComponentInteraction("unResolveReopenModal")]
    public async Task UnResolveReopenExecute()
    {
        var forumThread = await ((Context.Channel as PublicGuildThread)!).GetAsync();

        if (!forumThread.AppliedTags!.Contains(ConfigService.Get().SupportHelper.SupportResolveTagId))
        {
            await Context.Interaction.SendResponseAsync(
                InteractionCallback.Message(new InteractionMessageProperties()
                {
                    Flags = MessageFlags.Ephemeral,
                    Content = "-# <:reply:1430577881205182555> You cant re-open it twice!"
                })
            );
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
                            $"-# <:reply:1430577881205182555> **Thanks for contacting us again!**\n\n-# **Question from User**: \n-# {Context.Components
                                .OfType<Label>()
                                .Select(l => l.Component)
                                .OfType<TextInput>()
                                .FirstOrDefault(component => component.CustomId == "text")?.Value}"
                        )
                    ])
                ]
            }));

        var forumTags = new List<ulong>();
        if (forumThread.AppliedTags != null)
            foreach (var forumThreadAppliedTag in forumThread.AppliedTags)
            {
                if (forumThreadAppliedTag != ConfigService.Get().SupportHelper.SupportResolveTagId)
                    forumTags.Add(forumThreadAppliedTag);
            }

        await forumThread.ModifyAsync(options =>
        {
            options.Archived = false;
            options.AddAppliedTags(forumTags);
        });
    }
}