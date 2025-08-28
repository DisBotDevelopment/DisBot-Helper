using NetCord.Services.ComponentInteractions;

namespace DisBot_Helper_Bot.Interactions.Buttons;

public class ButtonModule : ComponentInteractionModule<ButtonInteractionContext>
{
    [ComponentInteraction("button")]
    public static string Button() => "You clicked a button!";
}