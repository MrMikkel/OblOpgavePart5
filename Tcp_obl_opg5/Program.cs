using System;

namespace Tcp_obl_opg5
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerWorker worker = new ServerWorker();
            worker.Start();

            Console.ReadLine();
        }
    }
}
