using System.Text;
using Discord;
using Discord.WebSocket;
using TAB2.Api;
using TAB2.Api.Interaction;
using TAB2.Api.Module;

namespace AntiPing;

[ModuleEntry(Name, Id, Version)]
public class ModuleMain : BaseModule
{
    public const string Name = "Anti Ping";
    public const string Id = "antiping";
    public const string Version = "1.0.0";

    public const string StrikeId = $"{Id}_strikes";

    private IBotInstance instance;
    private IDataManager dataManager;
    
    private Config config = new Config();
    private StrikeData strikeData = new StrikeData();

    public override void Initialize(IBotInstance instance)
    {
        this.instance = instance;
        dataManager = instance.DataManager;
        
        dataManager.RegisterData(Id, config);
        dataManager.RegisterData(StrikeId, config);
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
            .Where(x => x.Roles.Any(r => guildConfig.ContainsRole(r.Id)));

        SocketGuildUser? pingedUser = pingedUsers.FirstOrDefault();
        
        if (pingedUser != null)
        {
            GuildStrikeData guildStrikeData = strikeData.GetGuildData(guildChannel.Guild.Id);
            int strikes = guildStrikeData.GetStrike(message.Author.Id) + 1;
            guildStrikeData.SetStrike(message.Author.Id, strikes);
            
            await userMessage.ReplyAsync($"you can't ping {pingedUser.Username} dumbass, you now have {strikes} strikes");
        }
    }

    public override IEnumerator<Command> OnCommandRegister()
    {
        yield return new Command()
            .WithName("antiping")
            .WithDescription("Changes how Anti Ping behaves")
            .AddSubCommandGroup(new SubCommandGroup()
                .WithName("roles")
                .WithDescription("Add or remove anti ping roles")
                .AddSubCommand(new SubCommand()
                    .WithName("add")
                    .WithDescription("Add a role")
                    .AddArgument(new RoleArgument("role", "The role to add"))
                    .Executes(AddRole)
                )
                .AddSubCommand(new SubCommand()
                    .WithName("remove")
                    .WithDescription("Remove a role")
                    .AddArgument(new RoleArgument("role", "The role to remove"))
                    .Executes(RemoveRole)
                )
                .AddSubCommand(new SubCommand()
                    .WithName("list")
                    .WithDescription("List all anti ping roles")
                    .Executes(ListRoles)
                )
            );
    }

    private async Task ListRoles(ICommandContext context)
    {
        if (context.Channel is not SocketGuildChannel guildChannel)
        {
            await context.RespondAsync("This command can only be used in a guild!");
            return;
        }
        
        GuildConfig guildConfig = config.GetGuildConfig(guildChannel.Guild.Id);
        
        EmbedBuilder embedBuilder = new EmbedBuilder()
            .WithTitle("Anti Ping roles")
            .WithDescription(
                string.Join('\n', guildConfig
                        .GetPingRoles()
                        .Select(x => $"<@&{x}>")
                    )
                );

        await context.RespondAsync(embed: embedBuilder.Build());
    }
    
    private async Task AddRole(ICommandContext context)
    {
        if (context.Channel is not SocketGuildChannel guildChannel)
        {
            await context.RespondAsync("This command can only be used in a guild!");
            return;
        }

        if (context.GetArgument("role", out IRole? role))
        {
            GuildConfig guildConfig = config.GetGuildConfig(guildChannel.Guild.Id);
            guildConfig.AddRoleId(role.Id);
            dataManager.SaveData(Id);

            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle("Success")
                .WithDescription($"Added {role.Mention} to anti ping config!");
                
            await context.RespondAsync(embed: embedBuilder.Build());
        }
    }

    private async Task RemoveRole(ICommandContext context)
    {
        if (context.Channel is not SocketGuildChannel guildChannel)
        {
            await context.RespondAsync("This command can only be used in a guild!");
            return;
        }

        if (context.GetArgument("role", out IRole? role))
        {
            GuildConfig guildConfig = config.GetGuildConfig(guildChannel.Guild.Id);
            guildConfig.RemoveRoleId(role.Id);
            dataManager.SaveData(Id);
                
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle("Success")
                .WithDescription($"Removed {role.Mention} from anti ping config!");
                
            await context.RespondAsync(embed: embedBuilder.Build());
        }
    }
}