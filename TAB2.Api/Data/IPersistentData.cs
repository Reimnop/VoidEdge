using Cyotek.Data.Nbt;

namespace TAB2.Api.Data;

public interface IPersistentData
{
    void WriteData(TagCompound compound);
    void ReadData(TagCompound compound);
}