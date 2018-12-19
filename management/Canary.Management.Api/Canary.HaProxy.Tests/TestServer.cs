using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Canary.HaProxy.Tests
{
    public class TestServer : IDisposable
    {
        private readonly TcpListener listener;
        private readonly Task task;
        private readonly CancellationTokenSource cancel;
        public string RecievedText { get; set; } = string.Empty;
        public Dictionary<string, string> Response { get; set; }

        public TestServer(int port)
        {
            System.Diagnostics.Debug.WriteLine("open");
            this.cancel = new CancellationTokenSource();
            this.listener = new TcpListener(IPAddress.Loopback, port);
            this.listener.Start();
            this.task = Task.Run(async () => await this.Listen(this.cancel.Token));
        }

        private async Task Listen(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                if (!this.listener.Pending())
                {
                    continue;
                }
                var client = await this.listener.AcceptTcpClientAsync();
                using (var str = client.GetStream())
                {
                    var buf = new byte[1024];
                    bool firstWrite = true;
                    string prevText = string.Empty;
                    int msSinceLastCmd = 0;
                    while ((client.Connected || str.DataAvailable) && msSinceLastCmd < 500)
                    {
                        if (str.DataAvailable)
                        {
                            msSinceLastCmd = 0;
                            int dataRead = await str.ReadAsync(buf, 0, buf.Length, cancelToken);
                            string text = prevText + Encoding.ASCII.GetString(buf, 0, dataRead);
                            var cmds = text.Split('\n', ';');
                            prevText = cmds.Length == 1 ? string.Empty : cmds.Last();
                            this.RunCommands(cmds, str, ref firstWrite);
                        }
                        else
                        {
                            msSinceLastCmd += 10;
                            await Task.Delay(10, cancelToken);
                        }
                    }
                }
                client.Close();
            }
        }

        private void RunCommands(string[] cmds, Stream str, ref bool firstWrite)
        {
            foreach (var cmd in cmds.Select(s => s.Trim()).Where(s => s.Length > 0))
            {
                this.RecievedText += cmd + "\n";
                if (this.Response != null && this.Response.ContainsKey(cmd))
                {
                    var response = Encoding.ASCII.GetBytes(
                        firstWrite
                            ? this.Response[cmd]
                            : ("\n\n" + this.Response[cmd]));
                    str.Write(response, 0, response.Length);
                    firstWrite = false;
                }
            }
        }

        public void Dispose()
        {
            this.cancel.Cancel();
            this.task.Wait();
            this.listener.Stop();
            System.Diagnostics.Debug.WriteLine("close");
        }
    }
}
