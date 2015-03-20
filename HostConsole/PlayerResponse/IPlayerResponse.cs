namespace HostConsole.PlayerResponse
{
    internal interface IPlayerResponse
    {
        IPlayerResponse SetNext(IPlayerResponse playerResponse);
        string GetResponse(string action, string data);
    }
}