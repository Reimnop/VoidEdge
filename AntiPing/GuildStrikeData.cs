using Cyotek.Data.Nbt;

namespace AntiPing;

public class GuildStrikeData
{
    public readonly Dictionary<ulong, int> userStrikes = new Dictionary<ulong, int>();

    public void SetStrike(ulong userId, int strikes)
    {
        userStrikes[userId] = strikes;
    }

    public int GetStrike(ulong userId)
    {
        return userStrikes.TryGetValue(userId, out int value) ? value : 0;
    }

    public void WriteData(TagDictionary dictionary)
    {
        foreach (var (userId, strikes) in userStrikes)
        {
            dictionary.Add(userId.ToString(), strikes);
        }
    }

    public void ReadData(TagDictionary dictionary)
    {
        userStrikes.Clear();
        
        foreach (Tag tag in dictionary)
        {
            userStrikes.Add(ulong.Parse(tag.Name), (int) tag.GetValue());
        }
    }
}