using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Timers;
using System.Threading;

namespace MagicalPinServer
{
    class Program
    {
        static List<ServerInformation> serverList = new List<ServerInformation>();
        static System.Timers.Timer timer;

        class ServerInformation
        {
            public Socket RemoteSocket;
            public string ServerName;
            public string Status;
            public ServerInformation(Socket s, string name)
            {
                RemoteSocket = s;
                ServerName = name;
            }
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

        static void Main(string[] args)
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += RefreshDataDisplay;

            while (true)
            {
                Console.Clear();
                WTitle("魔针 MagicalPin 服务端 Beta 0.1");
                WL();
                WL();
                WL("\t1.\t添加监控服务器");
                WL("\t2.\t移除监控服务器");
                WL("\t3.\t查看监控服务器");
                WL("\t4.\t启动仪表盘");
                WL();
                WL();
                WL("请输入对应数字选择功能（1-4）：");
                switch (Console.ReadLine())
                {
                    case "1":
                        AddServer();
                        break;
                    case "2":
                        break;
                    case "3":
                        ListServer();
                        break;
                    case "4":
                        Monitoring();
                        break;
                    default:
                        break;
                }
            }
        }

        private static void RefreshDataDisplay(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
            WTitle("监控中 Monitoring");
            WL();
            WL();
            WL("服务器名\tCPU\tRAM");
            foreach (ServerInformation si in serverList)
            {
                WL(si.ServerName + "\t" + si.Status);
            }
        }

        static void AddServer()
        {
            Console.Clear();
            WTitle("添加监控服务器 Add Monitoring Server");
            WL();
            WL();
            WL("1. 请为该服务器命名：");
            string name = Console.ReadLine();
            WL("2. 请为该服务器指定一个本机监听端口（建议从 9996-10500 中选取）：");
            int port = int.Parse(Console.ReadLine());

            Console.Clear();
            WTitle("确认信息 Confirm The Information");
            WL();
            WL();
            WL("服务器名：\t" + name);
            WL("本机监听端口：\t" + port.ToString());
            WL();
            WL();
            WL("请确认（y/n）：");
            switch (Console.ReadLine())
            {
                case "y":
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    s.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
                    serverList.Add(new ServerInformation(s, name));
                    break;
                case "n":
                    return;
                default:
                    break;
            }
        }

        static void ListServer()
        {
            Console.Clear();
            WTitle("监控服务器列表 Monitoring Server List");
            WL();
            WL();
            WL("序号\t服务器名\t监听端口");

            int i = 1;
            foreach (ServerInformation s in serverList)
            {
                WL(i + "\t" + s.ServerName + "\t\t" + ((IPEndPoint)s.RemoteSocket.LocalEndPoint).Port.ToString());
                i++;
            }
            WL();
            WL();
            WL("按任意键返回首页。");
            Console.ReadLine();
        }

        static void Monitoring()
        {
            foreach (ServerInformation si in serverList)
            {
                Thread t = new Thread(delegate (object sio)
                {
                    ServerInformation si = sio as ServerInformation;
                    while (true)
                    {
                        byte[] buffer = new byte[102400];
                        EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                        int leng = si.RemoteSocket.ReceiveFrom(buffer, ref ep);
                        string msg = System.Text.Encoding.UTF8.GetString(buffer, 0, leng);
                        si.Status = msg;
                    }
                });
                t.Start(si);
            }
            timer.Start();
            while (true)
                ;
        }
    }
}
