using System;
using System.Net;
using System.Net.Sockets;

namespace MagicalPinServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9906));
            while (true)
            {
                byte[] buffer = new byte[102400];
                EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                int leng = s.ReceiveFrom(buffer, ref ep);
                string msg = System.Text.Encoding.UTF8.GetString(buffer, 0, leng);
                Console.WriteLine(msg);
            }
        }
    }
}
