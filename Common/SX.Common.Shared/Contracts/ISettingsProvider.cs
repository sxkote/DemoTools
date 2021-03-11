namespace SX.Common.Shared.Contracts
{
    public interface ISettingsProvider
    {
        string GetSettings(string name);
        T GetSettings<T>(string name);
    }
}
