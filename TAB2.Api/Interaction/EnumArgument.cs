namespace TAB2.Api.Interaction;

public class EnumArgument : Argument
{
    public List<(int, string)> Options { get; }

    public EnumArgument(string name, string description, bool isRequired = true) : base(name, description, isRequired)
    {
        Options = new List<(int, string)>();
    }

    public EnumArgument AddOption(int value, string name)
    {
        Options.Add((value, name));
        return this;
    }
}