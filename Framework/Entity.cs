using System.Runtime.CompilerServices;

namespace Biosphere {
    public abstract class Entity : BufferWriteable {
#pragma warning disable CS8618
        public Simulation World;
#pragma warning restore CS8618

        public bool remove = false;
        public bool plant = false;
        public int energy = 20;

        public Tile tile => World.GetTile(pos);

        public abstract string Type { get; }

        // Dictionary to represent all the states of a given entity
        // 0 is false, 1 is true
        private Dictionary<string, object> entityStates = new();

        public string state = "";

        public int delay = 0;
        private int _delay = 0;
        public void Step() {
            if (_delay <= 0) {
                _delay = delay;
                Update();
            }
            --_delay;
        }

        // id is automatically allocated, and position defaults to (0, 0) on surface
        // default direction is facing north
        public int id;
        private Vec3 pos;
        public Vec3 Pos {
            get { return pos; }
            set {
                if (!plant && !World.TraversablePos(value)) {
                    return;
                }
                if (plant && !World.ValidPos(value)) {
                    return;
                }
                if (IsOccupied(value)) {
                    return;
                }
                tile.planes[Pos.plane] = null;
                pos = value;
                tile.planes[Pos.plane] = this;

            }
        }
        public Vec3 direction = Vec3.up;

        private static int nextId = 0;

        private struct Node {
            public Vec3 parent;
            public float f, g, h;
            public bool closed;
        }
        private Node[]? nodes = null;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref Node GetNode(Vec3 pos) {
            return ref nodes![pos.y * World.width + pos.x];
        }
        private SortedSet<(float, Vec3)> openList = new SortedSet<(float, Vec3)>(Comparer<(float, Vec3)>.Create((a, b) => a.Item1.CompareTo(b.Item1)));
        public bool TryPathTo(out Vec3 dir, Vec3 goal, Func<Tile, bool>? condition = null, int depth = 5) {
            if (pos.x == goal.x && pos.y == goal.y) {
                dir = Vec3.zero;
                return true;
            }

            if (goal.x < 0 || goal.y < 0 || goal.x >= World.width || goal.y >= World.height) {
                dir = Vec3.zero;
                return false;
            }

            if (nodes == null || nodes.Length != World.size) {
                nodes = new Node[World.size];
            }

            for (int i = 0; i < nodes.Length; ++i) {
                nodes[i].parent = new Vec3(-1, -1, 0);
                nodes[i].f = float.MaxValue;
                nodes[i].g = float.MaxValue;
                nodes[i].h = float.MaxValue;
                nodes[i].closed = false;
            }

            ref Node start = ref GetNode(pos);
            start.f = 0.0f;
            start.g = 0.0f;
            start.h = 0.0f;
            start.parent = pos;

            openList.Clear();
            openList.Add((0.0f, pos));

            Vec3 current = pos;
            bool solved = false;
            int iteration = 0;
            while (openList.Count > 0 && (depth <= 0 || iteration++ < depth)) {
                (float f, Vec3 pos) n = openList.Min;
                openList.Remove(n);

                // Add this vertex to the closed list
                current = n.pos;
                GetNode(current).closed = true;

                // Generating all the 8 successors of this cell
                for (int i = -1; i <= 1; i++) {
                    for (int j = -1; j <= 1; j++) {
                        if (i == 0 && j == 0)
                            continue;

                        Vec3 newPos = new Vec3(current.x + i, current.y + j, 0);

                        // If this successor is a valid cell
                        if (newPos.x >= 0 && newPos.y >= 0 && newPos.x < World.width && newPos.y < World.height) {
                            ref Node nP = ref GetNode(newPos);

                            // If the destination cell is the same as the
                            // current successor
                            if (newPos.x == goal.x && newPos.y == goal.y) {
                                nP.parent = current;

                                current = newPos;
                                solved = true;
                                goto done;
                            }

                            // If the successor is already on the closed
                            // list or if it is blocked, then ignore it.
                            if (!nP.closed && (condition == null || condition(World.GetTile(newPos)))) {
                                float gNew = GetNode(current).g + (i == 0 || j == 0 ? 1.0f : 1.4f);
                                float hNew = Math.Abs(newPos.x - goal.x) + Math.Abs(newPos.y - goal.y);
                                float fNew = gNew + hNew;

                                // If it isn’t on the open list, add it to
                                // the open list. Make the current square
                                // the parent of this square. Record the
                                // f, g, and h costs of the square cell
                                if (nP.f == float.MaxValue || nP.f > fNew) {
                                    openList.Add((fNew, newPos));

                                    // Update the details of this cell
                                    nP.f = fNew;
                                    nP.g = gNew;
                                    nP.h = hNew;
                                    nP.parent = current;
                                }
                            }
                        }
                    }
                }
            }

        done:

            // Get dir
            Node now = GetNode(current);
            dir = current;
            while (now.parent.x != pos.x || now.parent.y != pos.y) {
                dir = now.parent;
                now = GetNode(now.parent);
            }

            dir.x -= pos.x;
            dir.y -= pos.y;

            return solved;
        }

        public bool IsOccupied(Vec3 vec) {
            return World.IsOccupied(vec) && World.GetTile(vec).planes[vec.plane] != this;
        }

#pragma warning disable CS8618
        public Entity() {
            id = GetId();
        }
#pragma warning restore CS8618

        private static int GetId() {
            return nextId++;
        }


        public void SetState(string stateName, object value) {
            if (entityStates.ContainsKey(stateName)) {
                entityStates[stateName] = value;
            } else {
                throw new InvalidNameException();
            }
        }
        public void AddState(string stateName, object value) {
            if (entityStates.ContainsKey(stateName)) {
                throw new AlreadyExistsException();
            } else {
                entityStates[stateName] = value;
            }
        }

        public object GetState(string stateName) {
            if (entityStates.ContainsKey(stateName)) {
                return entityStates[stateName];
            } else {
                throw new InvalidNameException();
            }
        }

        public bool HasState(string stateName) {
            return entityStates.ContainsKey(stateName);
        }

        // Runs every delay
        protected abstract void Update();

        // Runs every tick
        public virtual void FixedUpdate() { }

        public virtual void Write(ByteBuffer buffer) { }
    }
}
