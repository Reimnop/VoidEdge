using TAB2.Api.Data;

namespace TAB2.Api;

public interface IDataManager
{
    void RegisterData(string id, IPersistentData data);
    IPersistentData GetData(string id);
    void SaveData(string id);
}