namespace Framework {
    public abstract class Entity : BufferWriteable {
#pragma warning disable CS8618
        public Simulation World;
#pragma warning restore CS8618

        public Tile tile => World.GetTile(pos);

        public abstract string Type { get; }

        // Dictionary to represent all the states of a given entity
        // 0 is false, 1 is true
        public Dictionary<string, object> entityStates = new();

        // id is automatically allocated, and position defaults to (0, 0) on surface
        // default direction is facing north
        public int id;
        private Vec3 pos;
        public Vec3 Pos {
            get { return pos; }
            set {
                if (value.plane < 0 || value.plane > 3 || value.x < 0 || value.x > World.width - 1 || value.y < 0 || value.y > World.height - 1) {
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

        // Runs every tick
        public abstract void Update();

        public virtual void Write(ByteBuffer buffer) { }
    }
}
