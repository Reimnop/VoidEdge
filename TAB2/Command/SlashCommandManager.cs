using Discord;
using Discord.Commands.Builders;
using Discord.WebSocket;
using TAB2.Api.Interaction;

namespace TAB2.Command;

public class SlashCommandManager
{
    private readonly Dictionary<string, Api.Interaction.Command> commands = new Dictionary<string, Api.Interaction.Command>();
    private readonly DiscordSocketClient client;

    private List<SlashCommandBuilder>? commandBuilders = new List<SlashCommandBuilder>();
    private ApplicationCommandProperties[]? commandProperties;

    public SlashCommandManager(DiscordSocketClient client)
    {
        this.client = client;
    }

    public void RegisterCommand(Api.Interaction.Command command)
    {
        if (commandBuilders == null)
        {
            return;
        }
        
        SlashCommandBuilder commandBuilder = new SlashCommandBuilder()
            .WithName(command.Name)
            .WithDescription(command.Description);
            
        command.Arguments.ForEach(x => SetupArgument(commandBuilder, x));
        
        commandBuilders.Add(commandBuilder);
        commands.Add(command.Name, command);
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

    private void SetupArgument(SlashCommandBuilder builder, Argument info)
    {
        if (info is IntArgument)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.Integer, info.Description, info.IsRequired);
            return;
        }
        
        if (info is DoubleArgument)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.Number, info.Description, info.IsRequired);
            return;
        }
        
        if (info is StringArgument)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.String, info.Description, info.IsRequired);
            return;
        }
        
        if (info is UserArgument)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.User, info.Description, info.IsRequired);
            return;
        }
        
        if (info is RoleArgument)
        {
            builder.AddOption(info.Name, ApplicationCommandOptionType.Role, info.Description, info.IsRequired);
            return;
        }
        
        if (info is EnumArgument enumArgumentInfo)
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
        if (commands.TryGetValue(command, out Api.Interaction.Command? discordCommand))
        {
            if (discordCommand.ExecutesTaskDelegate != null)
            {
                return discordCommand.ExecutesTaskDelegate(context);
            }
        }
        
        return Task.CompletedTask;
    }
}