using System;
using System.Collections.Generic;
using System.Linq;
using HostConsole.DTO;
using Newtonsoft.Json;

namespace HostConsole
{
    public class FormDataParser : IFormDataParser
    {
        public string ActionParser(string data)
        {
            const string result = "NoAction"; ;

            foreach (var formDataItem in GetFormData(data))
            {
                var nameValue = formDataItem.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                switch (nameValue[0].Trim())
                {
                    case "name=\"action\"":
                        return nameValue[1];

                }
            }

            return result;
        }

        private static ICollection<string> GetFormData(string data)
        {
            if (string.IsNullOrEmpty(data)) return new List<string>();

            var contents = data.Split(new[] {"Content-Disposition: form-data;"}, StringSplitOptions.RemoveEmptyEntries);
            var partSeparator = contents[0].Replace("\r\n", string.Empty);
            contents = data.Split(new[] {partSeparator}, StringSplitOptions.RemoveEmptyEntries);

            var result = contents.Select(dataItem =>
                                       {
                                           var nameValue = dataItem.Split(new[] {"Content-Disposition: form-data;"}, StringSplitOptions.RemoveEmptyEntries);
                                           if (nameValue.Length > 1)
                                           {
                                               return nameValue[1];
                                           }

                                           return null;
                                       }).Where(item => item != null).ToList();
            return result;
        }

        public GameState GameStateParser(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            foreach (var formDataItem in GetFormData(input))
            {
                var nameValue = formDataItem.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                switch (nameValue[0].Trim())
                {
                    case "name=\"game_state\"":
                        return JsonConvert.DeserializeObject<GameState>(nameValue[1]);
                }
            }

            return null;
        }
    }
}