using Discord.WebSocket;

namespace TAB2.Api;

public interface IBotInstance
{
    DiscordSocketClient Client { get; }
    IDataManager DataManager { get; }
    ITaskScheduler TaskScheduler { get; }

    void Shutdown();
}