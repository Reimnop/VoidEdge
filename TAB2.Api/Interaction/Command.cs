using Discord;

namespace TAB2.Api.Interaction;

public delegate Task CommandExecutesTaskDelegate(ICommandContext commandContext);

public class Command
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public GuildPermission? Permission { get; set; } 
    public List<Argument> Arguments { get; } = new List<Argument>();
    public List<SubCommand> SubCommands { get; } = new List<SubCommand>();
    public List<SubCommandGroup> SubCommandGroups { get; } = new List<SubCommandGroup>();
    public CommandExecutesTaskDelegate? ExecutesTaskDelegate { get; set; }

    public Command WithName(string name)
    {
        Name = name;
        return this;
    }

    public Command WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public Command WithPermission(GuildPermission? permission)
    {
        Permission = permission;
        return this;
    }

    public Command AddArgument(Argument argument)
    {
        Arguments.Add(argument);
        return this;
    }

    public Command AddSubCommand(SubCommand subCommand)
    {
        SubCommands.Add(subCommand);
        return this;
    }
    
    public Command AddSubCommandGroup(SubCommandGroup subCommandGroup)
    {
        SubCommandGroups.Add(subCommandGroup);
        return this;
    }

    public Command Executes(CommandExecutesTaskDelegate executesTaskDelegate)
    {
        ExecutesTaskDelegate = executesTaskDelegate;
        return this;
    }
}