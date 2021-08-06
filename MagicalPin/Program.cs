using System;
using System.Timers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MagicalPin
{
    class Program
    {
        static Timer t;
        static Socket s;
        static IPEndPoint ipp;

        static void Main(string[] args)
        {
            t = new Timer(1000);
            t.AutoReset = true;
            t.Elapsed += T_Elapsed;

            WTitle("魔针 MagicalPin 客户端 Beta 0.1");
            WL();
            WL();
            WL("\t1.\t启动探针");
            WL("\t2.\t退出");
            WL();
            WL();
            WL("请输入对应数字选择功能（1/2）：");
            switch (Console.ReadLine())
            {
                case "1":
                    ReadyToConnect();
                    break;
                case "2":
                    return;
                default:
                    return;
            }
        }

        private static void T_Elapsed(object sender, ElapsedEventArgs e)
        {

            Console.Clear();
            WTitle("正在传递 Sending");

            s.SendTo(Encoding.UTF8.GetBytes(DateTime.Now.ToString()), ipp);
        }

        static void WL(string str = "", ConsoleColor fColor = ConsoleColor.White, ConsoleColor bColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = fColor;
            Console.BackgroundColor = bColor;
            Console.WriteLine(str);
        }

        static void WTitle(string titleStr)
        {
            WL("     " + titleStr + "     ", ConsoleColor.Yellow, ConsoleColor.DarkBlue);
        }

        static void Connect(string ip, string port)
        {
            ipp = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));//远端地址

            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//本地套接字
            s.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9905));

            t.Start();
            Pinning();
        }

        static void ReadyToConnect()
        {
            Console.Clear();
            WTitle("准备连接 Get Ready To Connect");
            WL();
            WL();
            WL("请输入主服务器 IP：");
            string ip = Console.ReadLine();
            WL("请输入主服务器监听端口：");
            string port = Console.ReadLine();

            Console.Clear();
            WTitle("确认信息 Confirm The Information");
            WL();
            WL();
            WL("主服务器 IP：" + ip);
            WL("主服务器监听端口：" + port);
            WL();
            WL();
            WL("请确认（y/n）：");
            switch (Console.ReadLine())
            {
                case "y":
                    Connect(ip, port);
                    break;
                case "n":
                    break;
                default:
                    break;
            }
        }

        static void Pinning()
        {
            while (true) ;
        }
    }
}
