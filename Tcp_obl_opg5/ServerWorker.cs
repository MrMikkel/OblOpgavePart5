using Football;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tcp_obl_opg5
{
    internal class ServerWorker
    {
        private const int PORT = 2121;

        private static List<FootballPlayer> list = new List<FootballPlayer>()
        {
            new FootballPlayer(1,"knud",123,23),
            new FootballPlayer(2,"knud",312,12),
            new FootballPlayer(3,"knud",231,7),
            new FootballPlayer(4,"knud",321,44),

        };

        public ServerWorker()
        {

        }

        internal void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Task.Run(
                    () =>
                    {
                        TcpClient tmpSocket = socket;
                        DoClient(tmpSocket);
                    }
                );
            }
        }

        private void DoClient(TcpClient socket)
        {
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                sw.AutoFlush = true;

                String cmd = sr.ReadLine();
                String data = sr.ReadLine();

                switch (cmd)
                {
                    case "hentAlle":
                        string jsonList = JsonSerializer.Serialize(list);
                        sw.WriteLine(jsonList);
                        break;

                    case "hent":
                        foreach(FootballPlayer f in list)
                        {
                            if(data == f.Id.ToString())
                            {
                                string jsonObj = JsonSerializer.Serialize(f);
                                sw.WriteLine(jsonObj);
                            }
                        }
                        break;

                    case "gem":
                        FootballPlayer footballPlayer = JsonSerializer.Deserialize<FootballPlayer>(data);
                        list.Add(footballPlayer);
                        break;

                    default:
                        break;
                }


                //sfdvsdvx car = JsonSerializer.Deserialize<Car>(carString);

                //Console.WriteLine("Received car json string " + carString);
                //Console.WriteLine("Received car : " + car);
            }
            socket?.Close();
        }
    }
}