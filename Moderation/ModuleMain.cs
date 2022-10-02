using Discord;
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
    
    public override void Initialize(IBotInstance instance)
    {
        Instance = this;
        
        botInstance = instance;
        dataManager = instance.DataManager;
        
        dataManager.RegisterData(Id, moderationData);
    }

    public EmbedBuilder StrikeUser(IGuildUser user, string reason)
    {
        ModerationUserData userData = moderationData.GetUserData(user.GuildId);
        int strikes = userData.GetUserStrikes(user.Id) + 1;
        userData.SetUserStrikes(user.Id, strikes);
        dataManager.SaveData(Id);

        return new EmbedBuilder()
            .WithTitle("Hey!")
            .WithDescription($"You were striked for **{reason}**\nYou now have {strikes} strikes")
            .WithFooter("Reaching 3 strikes will result in punishment!");
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