using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Canary.HaProxy
{
    public class BaseHandler
    {
        private readonly int port;
        private readonly string host;

        public BaseHandler(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        protected async Task<byte[]> SendTextAndReadResponse(string text)
        {
            var ms = new MemoryStream();
            var writeData = Encoding.ASCII.GetBytes(text + "\n");
            using (var client = new TcpClient(this.host, this.port))
            using (var stream = client.GetStream())
            {
                await stream.WriteAsync(writeData, 0, writeData.Length);
                byte[] readData = new byte[1024];
                int length;
                while ((length = await stream.ReadAsync(readData, 0, readData.Length)) > 0)
                {
                    await ms.WriteAsync(readData, 0, length);
                }
            }

            return ms.ToArray();
        }
    }
}
