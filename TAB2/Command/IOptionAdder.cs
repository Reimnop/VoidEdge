using Discord;

namespace TAB2.Command;

public interface IOptionAdder
{
    void AddOption(
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
        params ApplicationCommandOptionChoiceProperties[] choices);
}