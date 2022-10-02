using Cyotek.Data.Nbt;

namespace Moderation;

public class ModerationUserData
{
    private readonly Dictionary<ulong, int> strikes = new Dictionary<ulong, int>();
    
    public int GetUserStrikes(ulong userId)
    {
        if (strikes.TryGetValue(userId, out int value))
        {
            return value;
        }

        return 0;
    }

    public void SetUserStrikes(ulong userId, int value)
    {
        strikes[userId] = value;
    }
    
    public void WriteData(TagDictionary dictionary)
    {
        foreach (var (id, value) in strikes)
        {
            dictionary.Add(id.ToString(), value);
        }
    }

    public void ReadData(TagDictionary dictionary)
    {
        strikes.Clear();
        
        foreach (Tag tag in dictionary)
        {
            ulong id = ulong.Parse(tag.Name);
            strikes.Add(id, (int) tag.GetValue());
        }
    }
}