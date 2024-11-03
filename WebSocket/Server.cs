using Framework;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

// mutex for simultaneous editting

namespace Server {

    public class WorldHandler : WebSocketBehavior {
        private bool running = true;
        private bool added = false;
        private int temp = 1;
        private Simulation world;

        Task? currentTask = null;
        async Task MainHandler() {
            while (running) {
                Send(Server.worldList[Server.clientWorlds[ID]].Serialize());
                // Get Chris to write bytify
                // Send(Server.cursorList[Server.clientWorlds[ID]]);
                Thread.Sleep(1000);
            }
        }

        protected override void OnOpen() {
            base.OnOpen();
            Console.WriteLine($"New Connection! {ID}");
        }

        protected override void OnMessage(MessageEventArgs e) {
            // Need to figure out the format of e.data
            if (e.Type != Opcode.Binary) throw new Exception("ONLY BINARY");
            int i = 0;

            string type = BitHelper.ReadString(e.RawData, ref i);
            // CHRIS WRITE BIT BUFFER READING FOR EACH CASSE PLEASE THANKS UWU
            switch (type) {
                case "get":

                    int worldID = BitHelper.ReadInt(e.RawData, ref i);

                    if (currentTask == null) {

                        // Check what the content of the message is - need to examine e.data
                        if (Server.worldList.ContainsKey(worldID)) {
                            Server.clientWorlds.Add(ID, worldID);
                        } else {
                            while (!added) {
                                if (!Server.worldList.ContainsKey(temp)) {
                                    Server.worldList.Add(temp, new Simulation());
                                    Server.clientWorlds.Add(ID, temp);
                                    added = true;
                                } else {
                                    ++temp;
                                }
                            }
                        }

                        currentTask = MainHandler();
                    }
                    break;
                case "mutate":
                    // Need to figure out how data is formatted
                    // for (int i = 0; i < "Length of data list"; ++i;) {
                    for (int i = 0; i < 0; ++i) {
                        //e.data.x e.data.y
                        // Tile t = Server.worldList[Server.clientWorlds[ID]].GetTile(x, y);
                        // t.tileStates.clear();
                        // t.tileStates[] = e.data.states ?
                    }

                    break;
                case "cursor":
                    // e.data.x e.data.y
                    // Server.cursorList[Server.clientWorlds[ID]].Append(e.Data.x);
                    // Server.cursorList[Server.clientWorlds[ID]].Append(e.Data.y);
            }

            Console.WriteLine("Received from client: " + e);
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e) {
            Console.WriteLine($"Connection {ID} encountered error: {e}");
        }

        protected override void OnClose(CloseEventArgs e) {
            base.OnClose(e);
            Server.clientWorlds.Remove(ID);

        }
    }

    public class TestServer : WebSocketBehavior {
        protected override void OnOpen() {
            base.OnOpen();
            Console.WriteLine($"New Connection! {ID}");
        }

        protected override void OnMessage(MessageEventArgs e) {
            base.OnMessage(e);
            if (e.Type != Opcode.Binary) throw new Exception("ONLY BINARY");
            int i = 0;

            string type = BitHelper.ReadString(e.RawData, ref i);
            Vec3 point = BitHelper.ReadVector3(e.RawData, ref i);
        }
    }


    public class Server {

        // Client ID -> World ID
        public static Dictionary<string, int> clientWorlds = new Dictionary<string, int>();
        // World ID -> Simulation Object
        public static Dictionary<int, Simulation> worldList = new Dictionary<int, Simulation>();
        // World ID -> Integer array of Cursor Positions
        public static Dictionary<int, int[]> cursorList = new Dictionary<int, int[]>();

        public static WebSocketServer ws = new WebSocketServer("ws://127.0.0.1:8800");

        static void Main(string[] args) {

            // ws.AddWebSocketService<WorldHandler>("/");
            ws.AddWebSocketService<TestServer>("/");
            ws.Start();

            Console.ReadKey();

            ws.Stop();

        }
    }
}