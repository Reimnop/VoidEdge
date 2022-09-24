namespace TAB2.Api.Interaction;

public abstract class Argument
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsRequired { get; set; }

    public Argument(string name, string description, bool isRequired)
    {
        Name = name;
        Description = description;
        IsRequired = isRequired;
    }
}