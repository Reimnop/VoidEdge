using Discord;
using log4net;
using TAB2.Api;
using TAB2.Api.Interaction;
using TAB2.Api.Module;

namespace Moderation;

[ModuleEntry(Name, Id, Version)]
public class ModuleMain : BaseModule
{
    public const string Name = "Moderation";
    public const string Id = "moderation";
    public const string Version = "1.0.0";

    public static ModuleMain Instance { get; private set; }

    private ModerationData moderationData = new ModerationData();

    private IBotInstance botInstance;
    private IDataManager dataManager;

    private ILog log = LogManager.GetLogger(Id);
    
    public override void Initialize(IBotInstance instance)
    {
        Instance = this;
        
        botInstance = instance;
        dataManager = instance.DataManager;
        
        dataManager.RegisterData(Id, moderationData);
    }

    public async Task<StrikeResult> StrikeUserAsync(IGuildUser user, string reason)
    {
        ModerationUserData userData = moderationData.GetUserData(user.GuildId);
        int strikes = userData.GetUserStrikes(user.Id) + 1;

        bool punished = strikes >= 3;
        strikes = punished ? 0 : strikes;
        
        userData.SetUserStrikes(user.Id, strikes);
        dataManager.SaveData(Id);
        
        if (punished)
        {
            TimeSpan timeoutPeriod = TimeSpan.FromMinutes(15);

            try
            {
                await user.SetTimeOutAsync(timeoutPeriod);
            }
            catch (Exception e)
            {
                log.Warn($"Could not timeout user {user.Username}#{user.Discriminator}!", e);
            }

            TimestampTag tag = TimestampTag.FromDateTime(
                DateTime.UtcNow.Add(timeoutPeriod), 
                TimestampTagStyles.ShortTime);

            return new StrikeResult(
                new EmbedBuilder()
                    .WithTitle("Hey!")
                    .WithDescription($"You were **timed out** in **{user.Guild.Name}** until {tag} for **{reason}**\nPlease stop breaking **{user.Guild.Name}**'s server rules")
                    .WithFooter("Your strikes have been reset"), 
                punished);
        }

        return new StrikeResult(
            new EmbedBuilder()
                .WithTitle("Hey!")
                .WithDescription($"You were striked for **{reason}**\nYou now have {strikes} strike(s)")
                .WithFooter("Reaching 3 strikes will result in punishment!"), 
            punished);
    }

    public int GetUserStrikes(IGuildUser user)
    {
        ModerationUserData userData = moderationData.GetUserData(user.GuildId);
        return userData.GetUserStrikes(user.Id);
    }

    public override IEnumerator<Command> OnCommandRegister()
    {
        yield break;
    }
}