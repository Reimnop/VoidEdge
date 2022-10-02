using Discord;

namespace Moderation;

public static class ModerationAPI
{
    public static EmbedBuilder StrikeUser(IGuildUser user, string reason)
    {
        return ModuleMain.Instance.StrikeUser(user, reason);
    }
    
    public static int GetUserStrikes(IGuildUser user)
    {
        return ModuleMain.Instance.GetUserStrikes(user);
    }
}