using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TcpConnectScan
{
    class Program
    {
        static SemaphoreSlim semaphoreSlim;
        static int connect_timeout;
        static async Task connectp(int port , IPEndPoint ipe)
        {
            await semaphoreSlim.WaitAsync();
            var s = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            s.ConnectAsync(ipe);
            await Task.Delay(connect_timeout);

            if (s.Connected == true)
            {
                Console.WriteLine(ipe.Address+" Port :" + ipe.Port + " Open");   
            }
            s.Close();
            s.Dispose();
            semaphoreSlim.Release();
            return;
        } 
        static void usage()
        {
            Console.WriteLine("TcpConnectScan.exe ip_address port_start port_end nthreads connection_timeout(ms)"+Environment.NewLine+ "Example: TcpConnectScan.exe 10.1.1.1/24 1 500 10 50 ");
            return;
        }
        static void Main(string[] args)
        {
            
            
            Thread.Sleep(3000);
            List<Task> tasks = new List<Task>();
            string ip = args[0];
            IPAddress ipAddress = IPAddress.Parse(ip);
            int port_start=Int16.Parse(args[1]);
            int port_stop= Int16.Parse(args[2]);
            int th = Int16.Parse(args[3]);
            connect_timeout = Int16.Parse(args[4]);
            semaphoreSlim = new SemaphoreSlim(th);
            for (int i = port_start; i < port_stop; i++)
            {
                IPEndPoint e = new IPEndPoint(ipAddress, i);
                Task t=connectp(i,e);
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Done");
        }
    }
}
