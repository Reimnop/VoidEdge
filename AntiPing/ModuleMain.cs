using Discord;
using Discord.WebSocket;
using TAB2.Api;
using TAB2.Api.Command;
using TAB2.Api.Module;

namespace AntiPing;

[ModuleEntry(Name, Id, Version)]
public class ModuleMain : BaseModule
{
    public const string Name = "Anti Ping";
    public const string Id = "antiping";
    public const string Version = "1.0.0";

    private IBotInstance instance;
    private IDataManager dataManager;
    
    private Config config = new Config();

    public override void Initialize(IBotInstance instance)
    {
        this.instance = instance;
        dataManager = instance.DataManager;
        
        dataManager.RegisterData(Id, config);
    }

    public override async Task OnMessageReceived(SocketMessage message)
    {
        if (message.Author.IsBot)
        {
            return;
        }

        if (message is not SocketUserMessage userMessage)
        {
            return;
        }
        
        if (userMessage.Channel is not SocketGuildChannel guildChannel)
        {
            return;
        }
        
        GuildConfig guildConfig = config.GetGuildConfig(guildChannel.Guild.Id);
        
        IEnumerable<SocketGuildUser> pingedUsers = userMessage.MentionedUsers
            .OfType<SocketGuildUser>()
            .Where(x => x.Id != userMessage.Author.Id)
            .Where(x => x.Roles.Any(r => guildConfig.PingRoleIds.Contains(r.Id)));

        foreach (SocketGuildUser pingedUser in pingedUsers)
        {
            await userMessage.ReplyAsync($"you can't ping {pingedUser.Username} dumbass");
        }
    }

    public override IEnumerator<DiscordCommandInfo> OnCommandRegister()
    {
        yield return new DiscordCommandInfo()
            .WithName("antipingroles")
            .WithDescription("Changes how Anti Ping behaves")
            .AddEnumArgument("mode", "Add or Remove", x => x
                .AddOption(0, "Add")
                .AddOption(1, "Remove"))
            .AddRoleArgument("role", "The role which people who have it can not be pinged")
            .Executes(AntiPingConfig);
    }

    private async Task AntiPingConfig(ICommandContext context)
    {
        if (context.Channel is not SocketGuildChannel guildChannel)
        {
            await context.RespondAsync("This command can only be used in a guild!");
            return;
        }

        if (context.GetArgument("mode", out long mode) && context.GetArgument("role", out IRole? role))
        {
            GuildConfig guildConfig = config.GetGuildConfig(guildChannel.Guild.Id);

            if (mode == 0)
            {
                guildConfig.AddRoleId(role.Id);
                dataManager.SaveData(Id);

                EmbedBuilder embedBuilder = new EmbedBuilder()
                    .WithTitle("Success")
                    .WithDescription($"Added {role.Mention} to anti ping config!");
                
                await context.RespondAsync(embed: embedBuilder.Build());
            }
            else if (mode == 1)
            {
                guildConfig.RemoveRoleId(role.Id);
                dataManager.SaveData(Id);
                
                EmbedBuilder embedBuilder = new EmbedBuilder()
                    .WithTitle("Success")
                    .WithDescription($"Removed {role.Mention} from anti ping config!");
                
                await context.RespondAsync(embed: embedBuilder.Build());
            }
        }
    }
}