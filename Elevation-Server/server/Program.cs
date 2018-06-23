#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using db;
using db.data;
using log4net;
using log4net.Config;
using server.sfx;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;

#endregion

namespace server
{
    internal class Program
    {
        private static readonly List<HttpListenerContext> currentRequests = new List<HttpListenerContext>();

        private static HttpListener listener;

        internal static SimpleSettings Settings { get; set; }
        internal static XmlData GameData { get; set; }
        internal static Database Database { get; set; }
        internal static ILog Logger { get; } = LogManager.GetLogger("Server");

        internal static string InstanceId { get; set; }

        private static void Main(string[] args)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net_server.config"));

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            Settings = new SimpleSettings("server");
            Database = new Database(
                Settings.GetValue<string>("db_host", "127.0.0.1"),
                Settings.GetValue<string>("db_database", "rotmgprod"),
                Settings.GetValue<string>("db_user", "root"),
                Settings.GetValue<string>("db_auth", ""));
            GameData = new XmlData();

            InstanceId = Guid.NewGuid().ToString();
            Console.CancelKeyPress += (sender, e) => e.Cancel = true;

            var port = Settings.GetValue<int>("port", "80");

            if (RunPreCheck(port))
            {
                listener = new HttpListener();
                listener.Prefixes.Add($"http://*:{port}/");
                listener.Start();

                listener.BeginGetContext(ListenerCallback, null);
                Logger.Info($"Listening at port {port}...");
            }
            else
                Logger.Error($"Port {port} is occupied. Can't start listening...\nPress ESC to exit.");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Logger.Info("Terminating...");
            //To prevent a char/list account in use if
            //both servers are closed at the same time
            while (currentRequests.Count > 0);
            listener?.Stop();
            GameData.Dispose();
        }

        private static bool RunPreCheck(int port) => IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections().All(_ => _.LocalEndPoint.Port != port) && IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().All(_ => _.Port != port);

        private static void ListenerCallback(IAsyncResult ar)
        {
            try
            {
                if (!listener.IsListening) return;
                var context = listener.EndGetContext(ar);
                listener.BeginGetContext(ListenerCallback, null);
                ProcessRequest(context);
            }
            catch
            {
            }
        }

        private static void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                Logger.InfoFormat("Request \"{0}\" from: {1}",
                    context.Request.Url.LocalPath, context.Request.RemoteEndPoint);

                if (context.Request.Url.LocalPath.Contains("sfx") || context.Request.Url.LocalPath.Contains("music"))
                {
                    new Sfx().HandleRequest(context);
                    context.Response.Close();
                    return;
                }

                var s = "";
                if (context.Request.Url.LocalPath.IndexOf(".") == -1)
                    s = "server" + context.Request.Url.LocalPath.Replace("/", ".");
                else
                    s = "server" + context.Request.Url.LocalPath.Remove(context.Request.Url.LocalPath.IndexOf(".")).Replace("/", ".");

                var t = Type.GetType(s);
                var handler = Activator.CreateInstance(t, null, null);
                if (!(handler is RequestHandler))
                {
                    if (handler == null)
                        using (var wtr = new StreamWriter(context.Response.OutputStream))
                            wtr.Write("<Error>Class \"{0}\" not found.</Error>", t.FullName);
                    else
                        using (var wtr = new StreamWriter(context.Response.OutputStream))
                            wtr.Write("<Error>Class \"{0}\" is not of the type RequestHandler.</Error>", t.FullName);
                }
                else
                    (handler as RequestHandler).HandleRequest(context);
            }
            catch (Exception e)
            {
                currentRequests.Remove(context);
                using (var wtr = new StreamWriter(context.Response.OutputStream))
                    wtr.Write(e.ToString());
                Logger.Error(e);
            }

            context.Response.Close();
        }
    }
}