using Discord;
using Discord.Rest;
using Discord.WebSocket;
using TAB2.Api.Command;

using IMessage = Discord.IMessage;

namespace TAB2.Api.Module;

public abstract class BaseModule
{
    public abstract void Initialize(IBotInstance instance);

    public abstract IEnumerator<DiscordCommandInfo> OnCommandRegister();

    #region DefaultEvents
    public virtual Task OnReady() => Task.CompletedTask;

    public virtual Task OnChannelCreated(SocketChannel channel) => Task.CompletedTask;

    public virtual Task OnChannelDestroyed(SocketChannel channel) => Task.CompletedTask;

    public virtual Task OnChannelUpdated(SocketChannel oldChannel, SocketChannel newChannel) => Task.CompletedTask;

    public virtual Task OnMessageReceived(SocketMessage message) => Task.CompletedTask;

    public virtual Task OnMessageDeleted(Cacheable<IMessage, ulong> cacheableMessage, Cacheable<IMessageChannel, ulong> cacheableChannel) => Task.CompletedTask;

    public virtual Task OnMessagesBulkDeleted(IReadOnlyCollection<Cacheable<IMessage, ulong>> cacheableMessages, Cacheable<IMessageChannel, ulong> cacheableChannel) => Task.CompletedTask;

    public virtual Task OnMessageUpdated(Cacheable<IMessage, ulong> cacheableMessage, SocketMessage message, ISocketMessageChannel socketMessageChannel) => Task.CompletedTask;

    public virtual Task OnReactionAdded(Cacheable<IUserMessage, ulong> cacheableMessage, Cacheable<IMessageChannel, ulong> cacheableChannel, SocketReaction socketReaction) => Task.CompletedTask;

    public virtual Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cacheableMessage, Cacheable<IMessageChannel, ulong> cacheableChannel, SocketReaction socketReaction) => Task.CompletedTask;

    public virtual Task OnReactionsCleared(Cacheable<IUserMessage, ulong> cacheableMessage, Cacheable<IMessageChannel, ulong> cacheableChannel) => Task.CompletedTask;

    public virtual Task OnReactionsRemovedForEmote(Cacheable<IUserMessage, ulong> cacheableMessage, Cacheable<IMessageChannel, ulong> cacheableChannel, IEmote emote) => Task.CompletedTask;

    public virtual Task OnRoleCreated(SocketRole role) => Task.CompletedTask;

    public virtual Task OnRoleDeleted(SocketRole role) => Task.CompletedTask;

    public virtual Task OnRoleUpdated(SocketRole oldRole, SocketRole newRole) => Task.CompletedTask;

    public virtual Task OnJoinedGuild(SocketGuild guild) => Task.CompletedTask;

    public virtual Task OnLeftGuild(SocketGuild guild) => Task.CompletedTask;

    public virtual Task OnGuildAvailable(SocketGuild guild) => Task.CompletedTask;

    public virtual Task OnGuildUnavailable(SocketGuild guild) => Task.CompletedTask;

    public virtual Task OnGuildMembersDownloaded(SocketGuild guild) => Task.CompletedTask;

    public virtual Task OnGuildUpdated(SocketGuild oldGuild, SocketGuild newGuild) => Task.CompletedTask;

    public virtual Task OnGuildJoinRequestDeleted(Cacheable<SocketGuildUser, ulong> cacheableUser, SocketGuild guild) => Task.CompletedTask;

    public virtual Task OnGuildScheduledEventCreated(SocketGuildEvent guildEvent) => Task.CompletedTask;

    public virtual Task OnGuildScheduledEventUpdated(Cacheable<SocketGuildEvent, ulong> cacheableGuildEvent, SocketGuildEvent guildEvent) => Task.CompletedTask;

    public virtual Task OnGuildScheduledEventCancelled(SocketGuildEvent guildEvent) => Task.CompletedTask;

    public virtual Task OnGuildScheduledEventCompleted(SocketGuildEvent guildEvent) => Task.CompletedTask;

    public virtual Task OnGuildScheduledEventStarted(SocketGuildEvent guildEvent) => Task.CompletedTask;

    public virtual Task OnGuildScheduledEventUserAdd(Cacheable<SocketUser, RestUser, IUser, ulong> cacheable, SocketGuildEvent guildEvent) => Task.CompletedTask;

    public virtual Task OnGuildScheduledEventUserRemove(Cacheable<SocketUser, RestUser, IUser, ulong> cacheable, SocketGuildEvent guildEvent) => Task.CompletedTask;

    public virtual Task OnIntegrationCreated(IIntegration integration) => Task.CompletedTask;

    public virtual Task OnIntegrationUpdated(IIntegration integration) => Task.CompletedTask;

    public virtual Task OnIntegrationDeleted(IGuild guild, ulong guildId, Optional<ulong> integrationId) => Task.CompletedTask;

    public virtual Task OnUserJoined(SocketGuildUser user) => Task.CompletedTask;

    public virtual Task OnUserLeft(SocketGuild guild, SocketUser user) => Task.CompletedTask;

    public virtual Task OnUserBanned(SocketUser user, SocketGuild guild) => Task.CompletedTask;

    public virtual Task OnUserUnbanned(SocketUser user, SocketGuild guild) => Task.CompletedTask;

    public virtual Task OnUserUpdated(SocketUser oldUser, SocketUser newUser) => Task.CompletedTask;

    public virtual Task OnGuildMemberUpdated(Cacheable<SocketGuildUser, ulong> oldCacheableUser, SocketGuildUser newUser) => Task.CompletedTask;

    public virtual Task OnUserVoiceStateUpdated(SocketUser user, SocketVoiceState oldState, SocketVoiceState newState) => Task.CompletedTask;

    public virtual Task OnVoiceServerUpdated(SocketVoiceServer voiceServer) => Task.CompletedTask;

    public virtual Task OnCurrentUserUpdated(SocketSelfUser oldUser, SocketSelfUser newUser) => Task.CompletedTask;

    public virtual Task OnUserIsTyping(Cacheable<IUser, ulong> cacheableUser, Cacheable<IMessageChannel, ulong> cacheableChannel) => Task.CompletedTask;

    public virtual Task OnRecipientAdded(SocketGroupUser user) => Task.CompletedTask;

    public virtual Task OnRecipientRemoved(SocketGroupUser user) => Task.CompletedTask;

    public virtual Task OnPresenceUpdated(SocketUser user, SocketPresence oldPresence, SocketPresence newPresence) => Task.CompletedTask;

    public virtual Task OnInviteCreated(SocketInvite invite) => Task.CompletedTask;

    public virtual Task OnInviteDeleted(SocketGuildChannel channel, string invite) => Task.CompletedTask;

    public virtual Task OnInteractionCreated(SocketInteraction interaction) => Task.CompletedTask;

    public virtual Task OnButtonExecuted(SocketMessageComponent messageComponent) => Task.CompletedTask;

    public virtual Task OnSelectMenuExecuted(SocketMessageComponent messageComponent) => Task.CompletedTask;

    public virtual Task OnSlashCommandExecuted(SocketSlashCommand slashCommand) => Task.CompletedTask;

    public virtual Task OnUserCommandExecuted(SocketUserCommand command) => Task.CompletedTask;

    public virtual Task OnMessageCommandExecuted(SocketMessageCommand command) => Task.CompletedTask;

    public virtual Task OnAutocompleteExecuted(SocketAutocompleteInteraction interaction) => Task.CompletedTask;

    public virtual Task OnModalSubmitted(SocketModal modal) => Task.CompletedTask;

    public virtual Task OnApplicationCommandCreated(SocketApplicationCommand command) => Task.CompletedTask;

    public virtual Task OnApplicationCommandUpdated(SocketApplicationCommand command) => Task.CompletedTask;

    public virtual Task OnApplicationCommandDeleted(SocketApplicationCommand applicationCommand) => Task.CompletedTask;

    public virtual Task OnThreadCreated(SocketThreadChannel threadChannel) => Task.CompletedTask;

    public virtual Task OnThreadUpdated(Cacheable<SocketThreadChannel, ulong> cacheableThreadChannel, SocketThreadChannel threadChannel) => Task.CompletedTask;
    
    public virtual Task OnThreadDeleted(Cacheable<SocketThreadChannel, ulong> cacheableThreadChannel) => Task.CompletedTask;
    
    public virtual Task OnThreadMemberJoined(SocketThreadUser threadUser) => Task.CompletedTask;
    
    public virtual Task OnThreadMemberLeft(SocketThreadUser threadUser) => Task.CompletedTask;
    
    public virtual Task OnStageStarted(SocketStageChannel stageChannel) => Task.CompletedTask;
    
    public virtual Task OnStageEnded(SocketStageChannel stageChannel) => Task.CompletedTask;
    
    public virtual Task OnStageUpdated(SocketStageChannel oldStageChannel, SocketStageChannel newStageChannel) => Task.CompletedTask;
    
    public virtual Task OnRequestToSpeak(SocketStageChannel stageChannel, SocketGuildUser user) => Task.CompletedTask;
    
    public virtual Task OnSpeakerAdded(SocketStageChannel stageChannel, SocketGuildUser user) => Task.CompletedTask;

    public virtual Task OnSpeakerRemoved(SocketStageChannel stageChannel, SocketGuildUser user) => Task.CompletedTask;

    public virtual Task OnGuildStickerCreated(SocketCustomSticker sticker) => Task.CompletedTask;

    public virtual Task OnGuildStickerUpdated(SocketCustomSticker oldSticker, SocketCustomSticker newSticker) => Task.CompletedTask;

    public virtual Task OnGuildStickerDeleted(SocketCustomSticker sticker) => Task.CompletedTask;

    public virtual Task OnLog(LogMessage message) => Task.CompletedTask;

    public virtual Task OnLoggedIn() => Task.CompletedTask;

    public virtual Task OnLoggedOut() => Task.CompletedTask;
    #endregion
}