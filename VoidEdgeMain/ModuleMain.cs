using Discord;
using TAB2.Api;
using TAB2.Api.Command;
using TAB2.Api.Module;

namespace VoidEdgeMain;

[ModuleEntry(Name, Id, Version)]
public class ModuleMain : BaseModule
{
    public const string Name = "Void Edge";
    public const string Id = "voidedge";
    public const string Version = "1.0.0";

    private IBotInstance instance;
    
    public override void Initialize(IBotInstance instance)
    {
        this.instance = instance;
    }

    public override IEnumerator<DiscordCommandInfo> OnCommandRegister()
    {
        yield return new DiscordCommandInfo()
            .WithName("help")
            .WithDescription("Show bot help")
            .Executes(Help);
        
        yield return new DiscordCommandInfo()
            .WithName("ship")
            .WithDescription("Ship 2 people :flushed:")
            .AddUserArgument("user1", "The user to be shipped")
            .AddUserArgument("user2", "The user to be shipped to")
            .Executes(Ship);

        yield return new DiscordCommandInfo()
            .WithName("whether")
            .WithDescription("Whether we wanted it or not, we've stepped into a war with the Cabal on Mars.")
            .Executes(Whether);
        
        yield return new DiscordCommandInfo()
            .WithName("furry")
            .WithDescription("Rates the furriness of a user")
            .AddUserArgument("user", "The user to rate", false)
            .Executes(Furry);
    }

    private async Task Furry(ICommandContext context)
    {
        IUser user;
        if (!context.GetArgument("user", out user))
        {
            user = context.User;
        }

        if (user.Id == instance.Client.CurrentUser.Id)
        {
            await context.RespondAsync("I AM NOT A FURRY!! :rage:");
            return;
        }

        int score = (int) (user.Id % 101);
        await context.RespondAsync($"{user.Username} is {score}% furry :flushed:");
    }

    private async Task Whether(ICommandContext context)
    {
        await context.RespondAsync(
            "Whether we wanted it or not, we've stepped into a war with the Cabal on Mars. " + 
            "So let's get to taking out their command, one by one. Valus Ta'aurc. " +
            "From what I can gather he commands the Siege Dancers from an Imperial Land Tank outside of Rubicon. " +
            "He's well protected, but with the right team, we can punch through those defenses, take this beast out, " +
            "and break their grip on Freehold.");
    }

    private async Task Ship(ICommandContext context)
    {
        if (context.GetArgument("user1", out IUser? user1) && context.GetArgument("user2", out IUser? user2))
        {
            if (user1.Id == instance.Client.CurrentUser.Id && user1.Id == user2.Id)
            {
                await context.RespondAsync("shipping me with myself? :flushed:\ni'd rate it 11/10 ngl");
                return;
            }
            
            if (user1.Id == instance.Client.CurrentUser.Id)
            {
                await context.RespondAsync($"why are you shipping me with {user2.Username}, ew");
                return;
            }

            if (user2.Id == instance.Client.CurrentUser.Id)
            {
                await context.RespondAsync($"why are you shipping me with {user1.Username}, ew");
                return;
            }
            
            if (user1.Id == user2.Id)
            {
                await context.RespondAsync($"don't ship {user1.Username} to themselves that's gross!! :face_vomiting:");
                return;
            }
            
            int score = (int) ((user1.Id + user2.Id) % 11);

            string react;
            switch (score)
            {
                case >= 10:
                    react = "Congratulations!!! So romatic!! :tada:";
                    break;
                case >= 9 and < 10:
                    react = "holy hell!! both of you are perfect matches! :heart_eyes:";
                    break;
                case >= 7 and < 9:
                    react = "awesome! :flushed:";
                    break;
                case >= 5 and < 7:
                    react = "nice :wink:";
                    break;
                case >= 3 and < 5:
                    react = "quite mid ngl :neutral_face:";
                    break;
                default:
                    react = "ew wtf :confounded:";
                    break;
            }

            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle("Shipping 2 people")
                .WithDescription($"{user1.Mention} x {user2.Mention}\n{react} ({score}/10)");

            await context.RespondAsync(embed: builder.Build());
        }
    }

    private async Task Help(ICommandContext context)
    {
        EmbedBuilder embedBuilder = new EmbedBuilder()
            .WithTitle("Void Edge help panel")
            .WithDescription("Finding my commands? They're available as **slash commands** and you can find them by typing `/`")
            .WithCurrentTimestamp();
        
        await context.RespondAsync(embed: embedBuilder.Build());
    }
}