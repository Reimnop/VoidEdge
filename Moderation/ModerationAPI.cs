using Discord;

namespace Moderation;

public static class ModerationAPI
{
    public static Task<StrikeResult> StrikeUserAsync(IGuildUser user, string reason)
    {
        return ModuleMain.Instance.StrikeUserAsync(user, reason);
    }
    
    public static int GetUserStrikes(IGuildUser user)
    {
        return ModuleMain.Instance.GetUserStrikes(user);
    }
}