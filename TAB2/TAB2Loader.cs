using System.Text;
using Discord;
using Discord.WebSocket;
using log4net;
using TAB2.Api;
using TAB2.Api.Interaction;
using TAB2.Api.Module;
using TAB2.Command;
using TAB2.Module;

namespace TAB2;

public class TAB2Loader : IDisposable, IBotInstance
{
    public DiscordSocketClient Client { get; }
    public IDataManager DataManager => dataManager;
    public ITaskScheduler TaskScheduler => taskScheduler;

    private readonly ILog log;
    private readonly ModuleManager moduleManager;
    private readonly DataManager dataManager;
    private readonly TaskScheduler taskScheduler;
    private readonly SlashCommandManager slashCommandManager;

    private bool running = false;

    public TAB2Loader()
    {
        log = LogManager.GetLogger("Discord");
        
        DiscordSocketConfig config = new DiscordSocketConfig();
        config.DefaultRetryMode = RetryMode.AlwaysRetry;
        config.GatewayIntents = GatewayIntents.All;

        Client = new DiscordSocketClient(config);
        
        moduleManager = new ModuleManager(this);
        dataManager = new DataManager();
        taskScheduler = new TaskScheduler();
        slashCommandManager = new SlashCommandManager(Client);
    }

    public async Task Run(string token)
    {
        moduleManager.LoadModules("Modules", this);

        #region ModuleEvents
        // Module events
        // Everything is a one-liner
        Client.Ready += () => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReady());
        Client.ChannelCreated += channel => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnChannelCreated(channel));
        Client.ChannelDestroyed += channel => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnChannelDestroyed(channel));
        Client.ChannelUpdated += (oldChannel, newChannel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnChannelUpdated(oldChannel, newChannel));
        Client.GuildAvailable += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildAvailable(guild));
        Client.GuildUnavailable += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildUnavailable(guild));
        Client.GuildUpdated += (oldGuild, newGuild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildUpdated(oldGuild, newGuild));
        Client.GuildMembersDownloaded += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildMembersDownloaded(guild));
        Client.JoinedGuild += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnJoinedGuild(guild));
        Client.LeftGuild += guild => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLeftGuild(guild));
        Client.MessageDeleted += (message, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageDeleted(message, channel));
        Client.MessagesBulkDeleted += (messages, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessagesBulkDeleted(messages, channel));
        Client.MessageReceived += message => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageReceived(message));
        Client.MessageUpdated += (oldMessage, newMessage, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageUpdated(oldMessage, newMessage, channel));
        Client.ReactionAdded += (message, channel, reaction) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionAdded(message, channel, reaction));
        Client.ReactionRemoved += (message, channel, reaction) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionRemoved(message, channel, reaction));
        Client.ReactionsCleared += (message, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionsCleared(message, channel));
        Client.ReactionsRemovedForEmote += (message, channel, emote) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnReactionsRemovedForEmote(message, channel, emote));
        Client.UserBanned += (user, guild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserBanned(user, guild));
        Client.UserJoined += user => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserJoined(user));
        Client.UserLeft += (guild, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserLeft(guild, user));
        Client.UserUnbanned += (user, guild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserUnbanned(user, guild));
        Client.UserUpdated += (oldUser, newUser) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserUpdated(oldUser, newUser));
        Client.GuildMemberUpdated += (oldMember, newMember) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildMemberUpdated(oldMember, newMember));
        Client.UserVoiceStateUpdated += (user, oldState, newState) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserVoiceStateUpdated(user, oldState, newState));
        Client.VoiceServerUpdated += server => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnVoiceServerUpdated(server));
        Client.RoleCreated += role => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRoleCreated(role));
        Client.RoleDeleted += role => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRoleDeleted(role));
        Client.RoleUpdated += (oldRole, newRole) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRoleUpdated(oldRole, newRole));
        Client.GuildJoinRequestDeleted += (user, guild) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildJoinRequestDeleted(user, guild));
        Client.GuildScheduledEventCreated += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventCreated(guildEvent));
        Client.GuildScheduledEventUpdated += (oldEvent, newEvent) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventUpdated(oldEvent, newEvent));
        Client.GuildScheduledEventCancelled += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventCancelled(guildEvent));
        Client.GuildScheduledEventCompleted += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventCompleted(guildEvent));
        Client.GuildScheduledEventStarted += guildEvent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventStarted(guildEvent));
        Client.GuildScheduledEventUserAdd += (user, guildEvent) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventUserAdd(user, guildEvent));
        Client.GuildScheduledEventUserRemove += (user, guildEvent) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildScheduledEventUserRemove(user, guildEvent));
        Client.IntegrationCreated += integration => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnIntegrationCreated(integration));
        Client.IntegrationUpdated += integration => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnIntegrationUpdated(integration));
        Client.IntegrationDeleted += (guild, guildId, integrationId) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnIntegrationDeleted(guild, guildId, integrationId));
        Client.CurrentUserUpdated += (oldUser, newUser) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnCurrentUserUpdated(oldUser, newUser));
        Client.UserIsTyping += (user, channel) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserIsTyping(user, channel));
        Client.RecipientAdded += user => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRecipientAdded(user));
        Client.RecipientRemoved += user => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRecipientRemoved(user));
        Client.PresenceUpdated += (user, oldPresence, newPresence) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnPresenceUpdated(user, oldPresence, newPresence));
        Client.InviteCreated += invite => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnInviteCreated(invite));
        Client.InviteDeleted += (channel, invite) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnInviteDeleted(channel, invite));
        Client.InteractionCreated += interaction => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnInteractionCreated(interaction));
        Client.ButtonExecuted += messageComponent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnButtonExecuted(messageComponent));
        Client.SelectMenuExecuted += messageComponent => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSelectMenuExecuted(messageComponent));
        Client.SlashCommandExecuted += slashCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSlashCommandExecuted(slashCommand));
        Client.UserCommandExecuted += userCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnUserCommandExecuted(userCommand));
        Client.MessageCommandExecuted += messageCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnMessageCommandExecuted(messageCommand));
        Client.AutocompleteExecuted += autocomplete => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnAutocompleteExecuted(autocomplete));
        Client.ModalSubmitted += modal => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnModalSubmitted(modal));
        Client.ApplicationCommandCreated += applicationCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnApplicationCommandCreated(applicationCommand));
        Client.ApplicationCommandUpdated += applicationCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnApplicationCommandUpdated(applicationCommand));
        Client.ApplicationCommandDeleted += applicationCommand => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnApplicationCommandDeleted(applicationCommand));
        Client.ThreadCreated += thread => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadCreated(thread));
        Client.ThreadUpdated += (oldThread, newThread) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadUpdated(oldThread, newThread));
        Client.ThreadDeleted += thread => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadDeleted(thread));
        Client.ThreadMemberJoined += threadUser => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadMemberJoined(threadUser));
        Client.ThreadMemberLeft += threadUser => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnThreadMemberLeft(threadUser));
        Client.StageStarted += stage => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnStageStarted(stage));
        Client.StageEnded += stage => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnStageEnded(stage));
        Client.StageUpdated += (oldStage, newStage) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnStageUpdated(oldStage, newStage));
        Client.RequestToSpeak += (channel, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnRequestToSpeak(channel, user));
        Client.SpeakerAdded += (channel, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSpeakerAdded(channel, user));
        Client.SpeakerRemoved += (channel, user) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnSpeakerRemoved(channel, user));
        Client.GuildStickerCreated += customSicker => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildStickerCreated(customSicker));
        Client.GuildStickerUpdated += (oldSticker, newSticker) => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildStickerUpdated(oldSticker, newSticker));
        Client.GuildStickerDeleted += customSicker => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnGuildStickerDeleted(customSicker));
        Client.Log += log => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLog(log));
        Client.LoggedIn += () => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLoggedIn());
        Client.LoggedOut += () => moduleManager.RunOnAllModulesAsync(module => module.BaseModule.OnLoggedOut());
        #endregion
        
        running = true;
        
        Client.Log += ClientOnLog;
        Client.Ready += ClientOnReady;
        Client.SlashCommandExecuted += ClientOnSlashCommandExecuted;
        Client.JoinedGuild += ClientOnJoinedGuild;

        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();

        while (running)
        {
            taskScheduler.Tick();
            await Task.Delay(40);
        }
    }

    public void Shutdown()
    {
        Dispose();
    }

    private async Task ClientOnJoinedGuild(SocketGuild guild)
    {
        await slashCommandManager.RegisterGuild(guild);
    }

    private async Task ClientOnSlashCommandExecuted(SocketSlashCommand slashCommand)
    {
        SlashCommandContext context = new SlashCommandContext(slashCommand);
        // TODO: get subcommands...
        await slashCommandManager.RunCommand(context, slashCommand.CommandName);
    }

    private async Task ClientOnReady()
    {
        moduleManager.RunOnAllModules(module => RegisterCommands(module.BaseModule));
        await slashCommandManager.Freeze();
    }

    private void RegisterCommands(BaseModule module)
    {
        IEnumerator<Api.Interaction.Command> enumerator = module.OnCommandRegister();

        while (enumerator.MoveNext())
        {
            Api.Interaction.Command command = enumerator.Current;
            slashCommandManager.RegisterCommand(command);
        }
    }

    private Task ClientOnLog(LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Debug:
                log.Debug(msg.Message, msg.Exception);
                break;
            case LogSeverity.Info:
                log.Info(msg.Message, msg.Exception);
                break;
            case LogSeverity.Warning:
                log.Warn(msg.Message, msg.Exception);
                break;
            case LogSeverity.Error:
                log.Error(msg.Message, msg.Exception);
                break;
            case LogSeverity.Critical:
                log.Fatal(msg.Message, msg.Exception);
                break;
            case LogSeverity.Verbose:
                log.Debug(msg.Message, msg.Exception);
                break;
        }
        
        return Task.CompletedTask;
    }

    private void SplitCommand(string command, out string id, out string subCommand)
    {
        StringBuilder idBuilder = new StringBuilder();
        StringBuilder subCommandBuilder = new StringBuilder();

        int i = 0;
        
        // Get id
        for (; i < command.Length; i++)
        {
            if (command[i] == ' ')
            {
                break;
            }
            idBuilder.Append(command[i]);
        }
        
        // Skip spaces
        for (; i < command.Length; i++)
        {
            if (command[i] != ' ')
            {
                break;
            }
        }
        
        // Get the rest
        for (; i < command.Length; i++)
        {
            subCommandBuilder.Append(command[i]);
        }

        id = idBuilder.ToString();
        subCommand = subCommandBuilder.ToString();
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}