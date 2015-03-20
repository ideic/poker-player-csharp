namespace HostConsole.PlayerResponse
{
    public interface IPlayerResponse
    {
        IPlayerResponse SetNext(IPlayerResponse playerResponse);
        string GetResponse(string action, string data);
    }
}