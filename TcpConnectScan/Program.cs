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
        static async Task connectp(int port , IPEndPoint ipe)
        {
            await semaphoreSlim.WaitAsync();
            var s = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            await s.ConnectAsync(ipe);
            //await Task.Delay(5000);

            if (s.Connected == true)
            {
                Console.WriteLine(ipe.Address+" Port :" + ipe.Port + " Open");   
            }
            s.Close();
            s.Dispose();
            semaphoreSlim.Release();
            return;
        } 
        static void Main(string[] args)
        {
            Thread.Sleep(3000);
            List<Task> tasks = new List<Task>();
            string ip = "192.168.1.1";
            IPAddress ipAddress = IPAddress.Parse(ip);
            int port_start=10;
            int port_stop=3000;
            int th = 10;
            semaphoreSlim = new SemaphoreSlim(th);
            for (int i = 0; i < 500; i++)
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
