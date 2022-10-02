using Cyotek.Data.Nbt;
using TAB2.Api.Data;

namespace Moderation;

public class ModerationData : IPersistentData
{
    private readonly Dictionary<ulong, ModerationUserData> userDatas = new Dictionary<ulong, ModerationUserData>();

    public ModerationUserData GetUserData(ulong guildId)
    {
        if (userDatas.TryGetValue(guildId, out ModerationUserData? data))
        {
            return data;
        }

        ModerationUserData userData = new ModerationUserData();
        userDatas.Add(guildId, userData);

        return userData;
    }

    public void SetUserData(ulong guildId, ModerationUserData data)
    {
        userDatas[guildId] = data;
    }
    
    public void WriteData(TagCompound compound)
    {
        foreach (var (id, userData) in userDatas)
        {
            TagDictionary dictionary = new TagDictionary();
            userData.WriteData(dictionary);

            compound.Value.Add(id.ToString(), dictionary);
        }
    }

    public void ReadData(TagCompound compound)
    {
        userDatas.Clear();
        
        foreach (Tag tag in compound.Value)
        {
            ulong id = ulong.Parse(tag.Name);
            TagDictionary dictionary = (TagDictionary) tag.GetValue();
            ModerationUserData userData = new ModerationUserData();
            userData.ReadData(dictionary);
            
            userDatas.Add(id, userData);
        }
    }
}