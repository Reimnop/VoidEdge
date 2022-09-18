namespace TAB2.Api.Command;

public abstract class ArgumentInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsRequired { get; set; }

    public ArgumentInfo(string name, string description, bool isRequired)
    {
        Name = name;
        Description = description;
        IsRequired = isRequired;
    }
}