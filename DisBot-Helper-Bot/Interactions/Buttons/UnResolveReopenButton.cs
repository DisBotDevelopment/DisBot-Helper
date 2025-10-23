using DisBot_Helper_Bot.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace DisBot_Helper_Bot.Interactions.Buttons;

public class UnResolveReopenButton : ComponentInteractionModule<ButtonInteractionContext>
{
    private readonly ConfigService ConfigService;

    public UnResolveReopenButton(ConfigService configService)
    {
        ConfigService = configService;
    }

    [ComponentInteraction("unResolveReopen")]
    public async Task UnResolveReopenExecute()
    {
        var forumThread = await ((Context.Channel as PublicGuildThread)!).GetAsync();

        await Context.Interaction.SendResponseAsync(InteractionCallback.Modal(
            new ModalProperties("unResolveReopenModal", "Re-open your Post")
            {
                new LabelProperties("Why are you re-open your post?",
                    new TextInputProperties("text", TextInputStyle.Paragraph)
                    {
                        Required = true,
                        MaxLength = 500
                    })
                {
                    Description = "Write a short question and tell me why are you re-open this support post."
                }
            }));
    }
}