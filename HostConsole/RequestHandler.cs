using System;
using System.IO;
using System.Net;
using HostConsole.PlayerResponse;

namespace HostConsole
{
    internal class RequestHandler
    {
        private readonly IFormDataParser _formDataParser;
        private readonly IPlayerResponse _playerResponse;

        public RequestHandler()
        {
            _playerResponse = new CheckResponse();
            _playerResponse.SetNext(new VersionResponse())
                .SetNext(new BetRequest())
                .SetNext(new ShowDown());

            _formDataParser = new FormDataParser();
        }

        public void Process(HttpListener listener)
        {
            var context = listener.GetContext();
            var request = context.Request;
            var response = context.Response;

            var dataText = new StreamReader(request.InputStream, request.ContentEncoding).ReadToEnd();

            //functions used to decode json encoded data.
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //var data1 = Uri.UnescapeDataString(data_text);
            //string da = Regex.Unescape(data_text);
            // var unserialized = js.Deserialize(data_text, typeof(String));


            response.StatusCode = 200;
            response.StatusDescription = "OK";

            if (request.HasEntityBody && request.HttpMethod == "POST")
            {
                var cleanedData = WebUtility.UrlDecode(dataText);

                var action = _formDataParser.ActionParser(cleanedData);
                var gameResponse = _playerResponse.GetResponse(action, cleanedData);

                var buffer = request.ContentEncoding.GetBytes(gameResponse);
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            response.Close();
        }

       
    }

    internal class FormDataParser : IFormDataParser
    {
        public string ActionParser(string data)
        {
            int actionIndex = data.IndexOf("name=\"action\"");
            if (actionIndex > -1)
            {
                var values = data.Substring(actionIndex + 1).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                var action = values[1];

                return action;
            }

            return "NoAction";
        }
    }

    internal interface IFormDataParser
    {
        string ActionParser(string cleanedData);
    }
}
