using Discord;
using Discord.Commands.Builders;
using Discord.WebSocket;
using TAB2.Api.Command;

namespace TAB2.Command;

public class SlashCommandManager
{
    private readonly Dictionary<string, DiscordCommandInfo> commands = new Dictionary<string, DiscordCommandInfo>();
    private readonly DiscordSocketClient client;

    private List<SlashCommandBuilder>? commandBuilders = new List<SlashCommandBuilder>();
    private ApplicationCommandProperties[]? commandProperties;

    public SlashCommandManager(DiscordSocketClient client)
    {
        this.client = client;
    }

    public void RegisterCommand(DiscordCommandInfo commandInfo)
    {
        if (commandBuilders == null)
        {
            return;
        }
        
        SlashCommandBuilder commandBuilder = new SlashCommandBuilder()
            .WithName(commandInfo.Name)
            .WithDescription(commandInfo.Description);
            
        commandInfo.Arguments.ForEach(x => SetupArgument(commandBuilder, x));
        
        commandBuilders.Add(commandBuilder);
        commands.Add(commandInfo.Name, commandInfo);
    }

    public async Task Freeze()
    {
        if (commandBuilders == null)
        {
            return;
        }
        
        commandProperties = commandBuilders
            .Select(x => (ApplicationCommandProperties) x.Build())
            .ToArray();
        
        foreach (SocketGuild guild in client.Guilds)
        {
            await RegisterGuild(guild);
        }
    }

    public async Task RegisterGuild(SocketGuild guild)
    {
        if (commandProperties == null)
        {
            return;
        }
        
        await guild.BulkOverwriteApplicationCommandAsync(commandProperties);
    }

    private void SetupArgument(SlashCommandBuilder builder, ArgumentInfo info)
    {
        if (info is IntArgumentInfo)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.Integer, info.Description, info.IsRequired);
            return;
        }
        
        if (info is DoubleArgumentInfo)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.Number, info.Description, info.IsRequired);
            return;
        }
        
        if (info is StringArgumentInfo)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.String, info.Description, info.IsRequired);
            return;
        }
        
        if (info is UserArgumentInfo)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.User, info.Description, info.IsRequired);
            return;
        }
        
        if (info is EnumArgumentInfo enumArgumentInfo)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.Integer, info.Description, info.IsRequired, 
                choices: enumArgumentInfo.Options
                    .Select(x => new ApplicationCommandOptionChoiceProperties
                    {
                        Name = x.Item2,
                        Value = x.Item1
                    })
                    .ToArray());
            return;
        }
    }

    public Task RunCommand(string command, ICommandContext context)
    {
        if (commands.TryGetValue(command, out DiscordCommandInfo? discordCommand))
        {
            if (discordCommand.ExecutesTaskDelegate != null)
            {
                return discordCommand.ExecutesTaskDelegate(context);
            }
        }
        
        return Task.CompletedTask;
    }
}