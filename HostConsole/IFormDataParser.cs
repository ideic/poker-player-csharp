using HostConsole.DTO;

namespace HostConsole
{
    public interface IFormDataParser
    {
        string ActionParser(string cleanedData);
        GameState GameStateParser(string input);
    }
}