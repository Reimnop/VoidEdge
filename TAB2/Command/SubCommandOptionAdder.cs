using Discord;

namespace TAB2.Command;

public class SubCommandOptionAdder : IOptionAdder
{
    private readonly SlashCommandOptionBuilder subCommandBuilder;

    public SubCommandOptionAdder(SlashCommandOptionBuilder commandBuilder)
    {
        subCommandBuilder = commandBuilder;
    }
    
    public void AddOption(
        string name, 
        ApplicationCommandOptionType type, 
        string description, 
        bool? isRequired = null,
        bool isDefault = false, 
        bool isAutocomplete = false, 
        double? minValue = null, 
        double? maxValue = null,
        List<SlashCommandOptionBuilder> options = null, 
        List<ChannelType> channelTypes = null, 
        params ApplicationCommandOptionChoiceProperties[] choices)
    {
        subCommandBuilder.AddOption(name, type, description, isRequired, isDefault, isAutocomplete, minValue,
            maxValue, options, channelTypes, choices);
    }
}