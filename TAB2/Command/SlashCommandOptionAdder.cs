using Discord;

namespace TAB2.Command;

public class SlashCommandOptionAdder : IOptionAdder
{
    private readonly SlashCommandBuilder slashCommandBuilder;

    public SlashCommandOptionAdder(SlashCommandBuilder commandBuilder)
    {
        slashCommandBuilder = commandBuilder;
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
        slashCommandBuilder.AddOption(name, type, description, isRequired, isDefault, isAutocomplete, minValue,
            maxValue, options, channelTypes, choices);
    }
}