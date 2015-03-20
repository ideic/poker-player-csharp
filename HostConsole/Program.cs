using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HostConsole
{
    class Program
    {
        private static HttpListener _listener;
        private static readonly RequestHandler Request = new RequestHandler();

        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            var port = 1234;
            if (args.Length > 0)
            {
                int.TryParse(args[0], out port);
            }

            StartHttPListener(string.Format("http://*:{0}/", port));
        }

        private static void StartHttPListener(string url)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(url);
            _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            _listener.Start();

            var cts = new CancellationTokenSource();
            var taskListener = new Task(() => Listener(cts.Token), cts.Token);
            taskListener.Start();

            Console.WriteLine("Started");
            taskListener.Wait(cts.Token);
            _listener.Stop();
        }

        private static void Listener(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    Request.Process(_listener);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }
    }
}
