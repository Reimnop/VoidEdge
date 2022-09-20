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

    public override IEnumerator<DiscordCommandInfo> OnCommandRegister()
    {
        yield return new DiscordCommandInfo()
            .WithName("antipingconfig")
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
        
        if (context.GetArgument("mode", out int? mode) && context.GetArgument("role", out IRole? role))
        {
            GuildConfig guildConfig = config.GetGuildConfig(guildChannel.Guild.Id);
            
            if (mode == 0)
            {
                guildConfig.AddRoleId(role.Id);
                await context.RespondAsync($"Added {role.Mention} to anti ping config!");
            }
            else if (mode == 1)
            {
                guildConfig.RemoveRoleId(role.Id);
                await context.RespondAsync($"Removed {role.Mention} from anti ping config!");
            }
        }
    }
}