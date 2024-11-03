using System;
using System.Net.WebSockets;
using WebSocketSharp;

namespace Client {
    public class Program {
        public static void Main(string[] args) {
            using (var ws = new WebSocketSharp.WebSocket("ws://localhost:8800")) {
                ws.OnMessage += (sender, e) =>
                                  Console.WriteLine("Laputa says: " + e.Data);

                ws.Connect();
                ws.Send("BALUS");
                Console.ReadKey(true);
            }
        }
    }
}