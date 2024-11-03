using Biosphere;
using WebSocketSharp;
using WebSocketSharp.Server;

// mutex for simultaneous editting

namespace Server {

    public class WorldHandler : WebSocketBehavior {
        protected override void OnOpen() {
            base.OnOpen();
            Console.WriteLine($"New Connection! {ID}");
        }

        private SimulationInstance? instance = null;
        private async Task Handler() {
            if (instance == null) return;

            string code = instance.code;
            SimulationInstance _instance = instance;

            while (instance != null) {
                await _instance.mutex.WaitAsync();
                try {
                    Send(_instance.sim.Serialize());
                } finally {
                    _instance.mutex.Release();
                }
                Thread.Sleep(100);
            }

            Console.WriteLine($"{ID} Left World {code}");
        }

        protected override void OnMessage(MessageEventArgs e) {
            // Need to figure out the format of e.data
            if (!e.IsBinary) throw new Exception("ONLY BINARY");
            int i = 0;

            string type = BitHelper.ReadString(e.RawData, ref i);

            switch (type) {
            case "get": {
                if (instance != null) return;

                string code = BitHelper.ReadString(e.RawData, ref i);
                if (!Server.worlds.ContainsKey(code)) {
                    Simulation sim = new Simulation(20, 20);

                    // Generate world...
                    //sim.AddSystem(new CloudSpawner());
                    sim.AddSystem(new TreeSpawner());
                    sim.AddSystem(new BirdSpawner());
                    sim.AddSystem(new DogSpawner());

                    sim.AddRule(new Fire());
                    sim.AddRule(new Rain());

                    Server.worlds.Add(code, sim);
                    Console.WriteLine($"Created World {code}");
                }

                if (!Server.runningWorlds.ContainsKey(code)) {
                    SimulationInstance inst = new SimulationInstance(code, Server.worlds[code]);
                    inst.Start();
                    Server.runningWorlds.Add(code, inst);
                    Console.WriteLine($"Started World {code}");
                }

                instance = Server.runningWorlds[code];
                lock (instance.refMutex) {
                    instance.refCount += 1;
                }

                if (!Server.runningWorlds.ContainsKey(code)) {
                    instance.Start();
                    Server.runningWorlds.Add(code, instance);
                    Console.WriteLine($"Started World {code}");
                }

                Task.Run(Handler);
                Console.WriteLine($"{ID} Joined World {code}");
            }
            break;
            }
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e) {
            Console.WriteLine($"Connection {ID} encountered error: {e}");
        }

        protected override void OnClose(CloseEventArgs e) {
            base.OnClose(e);

            Console.WriteLine($"Closed Connection! {ID}");

            if (instance == null) return;
            lock (instance.refMutex) {
                instance.refCount -= 1;
            }
            instance = null;
        }
    }

    public class SimulationInstance {
        public readonly SemaphoreSlim mutex = new(1, 1);

        public readonly object refMutex = new();
        private int _refCount = 0;
        public int refCount {
            get {
                return _refCount;
            }
            set {
                _refCount = value;
                Console.WriteLine($"World {code} has {_refCount} references.");
                if (_refCount <= 0) {
                    Stop();
                }
            }
        }

        private bool running = true;

        public readonly Simulation sim;
        public readonly string code;
        public SimulationInstance(string code, Simulation sim) {
            this.code = code;
            this.sim = sim;
        }

        private async Task Run() {
            while (running) {
                await mutex.WaitAsync();
                try {
                    sim.Step();
                } finally {
                    mutex.Release();
                }
                Thread.Sleep(100);
            }
        }

        private Task? current = null;
        public void Start() {
            if (current != null) return;
            current = Task.Run(Run);
        }

        public void Stop() {
            if (current == null) return;
            running = false;
            current.Wait();
            current = null;
            Server.runningWorlds.Remove(code);
            Console.WriteLine($"Ended World {code}");
        }
    }

    public class Server {
        public static WebSocketServer ws = new WebSocketServer("ws://127.0.0.1:8800");
        public static Dictionary<string, Simulation> worlds = new Dictionary<string, Simulation>();
        public static Dictionary<string, SimulationInstance> runningWorlds = new Dictionary<string, SimulationInstance>();

        static void Main(string[] args) {
            ws.AddWebSocketService<WorldHandler>("/");
            ws.Start();

            Console.ReadKey();

            ws.Stop();
        }
    }
}