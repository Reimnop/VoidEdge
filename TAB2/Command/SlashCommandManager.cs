using Discord;
using Discord.WebSocket;
using TAB2.Api.Interaction;

namespace TAB2.Command;

public class SlashCommandManager
{
    private readonly Dictionary<MultiStringHash, CommandExecutesTaskDelegate> executesTasks = new Dictionary<MultiStringHash, CommandExecutesTaskDelegate>();
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
        
        foreach (SubCommandGroup subCommandGroup in command.SubCommandGroups)
        {
            SlashCommandOptionBuilder subCommandGroupBuilder = new SlashCommandOptionBuilder()
                .WithName(subCommandGroup.Name)
                .WithDescription(subCommandGroup.Description)
                .WithType(ApplicationCommandOptionType.SubCommandGroup);

            foreach (SubCommand subCommand in subCommandGroup.SubCommands)
            {
                SlashCommandOptionBuilder subCommandBuilder = GetSubCommandBuilder(subCommand);
            
                AddCommandTask(subCommand.ExecutesTaskDelegate, command.Name, subCommandGroup.Name, subCommand.Name);
                subCommandGroupBuilder.AddOption(subCommandBuilder);
            }

            commandBuilder.AddOption(subCommandGroupBuilder);
        }

        foreach (SubCommand subCommand in command.SubCommands)
        {
            SlashCommandOptionBuilder subCommandBuilder = GetSubCommandBuilder(subCommand);

            AddCommandTask(subCommand.ExecutesTaskDelegate, command.Name, subCommand.Name);
            commandBuilder.AddOption(subCommandBuilder);
        }

        SlashCommandOptionAdder optionAdder = new SlashCommandOptionAdder(commandBuilder);
        command.Arguments.ForEach(x => SetupArgument(optionAdder, x));
        
        commandBuilders.Add(commandBuilder);
        AddCommandTask(command.ExecutesTaskDelegate, command.Name);
    }

    private SlashCommandOptionBuilder GetSubCommandBuilder(SubCommand subCommand)
    {
        SlashCommandOptionBuilder subCommandBuilder = new SlashCommandOptionBuilder()
            .WithName(subCommand.Name)
            .WithDescription(subCommand.Description)
            .WithType(ApplicationCommandOptionType.SubCommand);

        SubCommandOptionAdder subCommandOptionAdder = new SubCommandOptionAdder(subCommandBuilder);
        foreach (Argument argument in subCommand.Arguments)
        {
            SetupArgument(subCommandOptionAdder, argument);
        }

        return subCommandBuilder;
    }

    private void AddCommandTask(CommandExecutesTaskDelegate? taskDelegate, params string[] names)
    {
        if (taskDelegate != null)
        {
            executesTasks.Add(new MultiStringHash(names), taskDelegate);
        }
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

    private void SetupArgument(IOptionAdder adder, Argument argument)
    {
        if (argument is IntArgument)
        {
            adder.AddOption(argument.Name, ApplicationCommandOptionType.Integer, argument.Description, argument.IsRequired);
            return;
        }
        
        if (argument is DoubleArgument)
        {
            adder.AddOption(argument.Name, ApplicationCommandOptionType.Number, argument.Description, argument.IsRequired);
            return;
        }
        
        if (argument is StringArgument)
        {
            adder.AddOption(argument.Name, ApplicationCommandOptionType.String, argument.Description, argument.IsRequired);
            return;
        }
        
        if (argument is UserArgument)
        {
            adder.AddOption(argument.Name, ApplicationCommandOptionType.User, argument.Description, argument.IsRequired);
            return;
        }
        
        if (argument is RoleArgument)
        {
            adder.AddOption(argument.Name, ApplicationCommandOptionType.Role, argument.Description, argument.IsRequired);
            return;
        }
        
        if (argument is EnumArgument enumArgument)
        {
            adder.AddOption(enumArgument.Name, ApplicationCommandOptionType.Integer, enumArgument.Description, enumArgument.IsRequired, 
                choices: enumArgument.Options
                    .Select(x => new ApplicationCommandOptionChoiceProperties
                    {
                        Name = x.Item2,
                        Value = x.Item1
                    })
                    .ToArray());
            return;
        }
    }

    public Task RunCommand(ICommandContext context)
    {
        MultiStringHash nameHash = context.GetCommandNameHash();
        if (executesTasks.TryGetValue(nameHash, out CommandExecutesTaskDelegate? taskDelegate))
        {
            return taskDelegate(context);
        }
        return Task.CompletedTask;
    }
}