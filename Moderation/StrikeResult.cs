using Discord;

namespace Moderation;

public struct StrikeResult
{
    public EmbedBuilder Message { get; }
    public bool Punished { get; }

    public StrikeResult(EmbedBuilder message, bool punished)
    {
        Message = message;
        Punished = punished;
    }
}