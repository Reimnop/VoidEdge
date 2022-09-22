namespace TAB2.Api.Command;

public class EnumArgumentInfo : ArgumentInfo
{
    public List<(int, string)> Options { get; }

    public EnumArgumentInfo(string name, string description, bool isRequired = true) : base(name, description, isRequired)
    {
        Options = new List<(int, string)>();
    }

    public EnumArgumentInfo AddOption(int value, string name)
    {
        Options.Add((value, name));
        return this;
    }
}