namespace TAB2.Api.Interaction;

public class SubCommandGroup
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SubCommand> SubCommands { get; } = new List<SubCommand>();
    
    public SubCommandGroup WithName(string name)
    {
        Name = name;
        return this;
    }

    public SubCommandGroup WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public SubCommandGroup AddSubCommand(SubCommand subCommand)
    {
        SubCommands.Add(subCommand);
        return this;
    }
}