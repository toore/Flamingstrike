using System;
using System.Net;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace FlamingStrike.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.ReadKey();

            //var hubConnection = new HubConnection("http://localhost:60643/hubs/gameengine", false);

            var hubConnection = new HubConnectionBuilder()
                //.WithUrl("http://localhost:60643/hubs/gameengine")
                .WithUrl("https://localhost:44391/hubs/gameengine")
                .WithConsoleLogger(LogLevel.Trace)
                .Build();

            hubConnection.On<string, string>("SendAction", Print);
            //ServicePointManager.DefaultConnectionLimit = 10;

            hubConnection.StartAsync().Wait();

            Console.ReadKey();
        }

        private static void Print(string a, string b)
        {
            Console.WriteLine($"{a} {b}");
        }
    }
}