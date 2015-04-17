using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HostConsole.DTO;
using Newtonsoft.Json;

namespace HostConsole
{
    public class FormDataParser : IFormDataParser
    {
        public string ActionParser(string data)
        {
            return InputParser(data, "action", "NoAction");
        }

        private static IEnumerable<string> GetFormData(string data)
        {
            return data.Replace("'", "").Split(new []{"&"}, StringSplitOptions.RemoveEmptyEntries);
        }

        public GameState GameStateParser(string input)
        {
            var parseResult = InputParser(input, "game_state", null);

            if (string.IsNullOrEmpty(parseResult)) return null;

            return JsonConvert.DeserializeObject<GameState>(parseResult);
        }

        private string InputParser(string data, string token, string defaultValue)
        {
            if (string.IsNullOrEmpty(data)) return defaultValue;

            foreach (var formDataItem in GetFormData(data))
            {
                var nameValue = formDataItem.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (nameValue[0].Trim() == token) return nameValue[1];
            }

            return defaultValue;
        }
    }
}