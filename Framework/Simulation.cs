namespace Biosphere {
    public class Simulation {
        public Dictionary<string, BufferWriteable?> worldStates = new();

        public Tile[] tiles;
        public List<System> systems = new();
        public List<Rule> rules = new();
        public HashSet<Entity> entities = new();
        private HashSet<Entity> _entities = new();
        public int[] queuedLocations;
        private Dictionary<Entity, Vec3> queuedEntities = new();

        public readonly int height;
        public readonly int width;
        public readonly int size;

        private int stepCount = 0;
        public int step { get { return stepCount; } }

        public Simulation(int width = 10, int height = 10) {
            this.height = height;
            this.width = width;
            size = this.height * this.width;
            tiles = new Tile[size];
            queuedLocations = new int[size];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    tiles[y * width + x] = new Tile(new Vec3(x, y, 0));
                }
            }
        }

        public void SetState(string stateName, BufferWriteable value) {
            if (worldStates.ContainsKey(stateName)) {
                worldStates[stateName] = value;
            } else {
                throw new InvalidNameException();
            }
        }
        public void AddState(string stateName, BufferWriteable value) {
            if (worldStates.ContainsKey(stateName)) {
                throw new AlreadyExistsException();
            } else {
                worldStates[stateName] = value;
            }
        }

        public BufferWriteable? GetState(string stateName) {
            if (worldStates.ContainsKey(stateName)) {
                return worldStates[stateName];
            } else {
                throw new InvalidNameException();
            }
        }

        public bool HasState(string stateName) {
            return worldStates.ContainsKey(stateName);
        }

        // Error when entity is placed somewhere it cannot spawn (already occupied)
        public void AddEntity<T>(T entity, Vec3 pos) where T : Entity {
            if (entity.World == this) throw new AlreadyExistsException();

            if (IsOccupied(pos)) {
                throw new AlreadyOccupiedException();
            } else {
                entity.World = this;
                queuedLocations[pos.y * width + pos.x] |= 0b1 << pos.plane;
                queuedEntities.Add(entity, pos);
            }
        }

        public void RemoveEntity<T>(T entity) where T : Entity {
            entity.remove = true;
        }

        public Tile GetTile(Vec3 vec) {
            return tiles[vec.y * width + vec.x];
        }

        public bool ValidPos(Vec3 vec) {
            return vec.x >= 0 && vec.y >= 0 && vec.x < width && vec.y < height && vec.plane >= 0 && vec.plane < Tile.NumPlanes;
        }

        public bool TraversablePos(Vec3 vec) {
            return vec.x >= 0 && vec.y >= 0 && vec.x < width && vec.y < height && vec.plane >= 0 && vec.plane < Tile.TraversablePlanes;
        }

        public bool IsOccupied(Vec3 vec) {
            if (!ValidPos(vec)) return false;
            int field = 0b1 << vec.plane;
            int loc = queuedLocations[vec.y * width + vec.x];
            if ((field & loc) == field) return true;
            return tiles[vec.y * width + vec.x].planes[vec.plane] != null;
        }


        public void AddSystem<T>(T system) where T : System {
            system.World = this;
            systems.Add(system);
        }

        public void AddSystems<T>(T[] systemsArr) where T : System {
            foreach (System system in systemsArr) {
                AddSystem(system);
            }
        }

        public void AddRule<T>(T rule) where T : Rule {
            rule.World = this;
            rules.Add(rule);
        }

        public void AddRules<T>(T[] rulesArr) where T : Rule {
            foreach (Rule rule in rulesArr) {
                AddRule(rule);
            }
        }

        public void Step() {
            foreach (Tile tile in tiles) {
                foreach (Rule rule in rules) {
                    rule.Each(tile);
                }
            }

            foreach (System system in systems) {
                system.Step();
            }

            _entities.Clear();
            foreach (Entity entity in entities) {
                entity.FixedUpdate();
                entity.Step();
                if (!entity.remove) _entities.Add(entity);
                else GetTile(entity.Pos).planes[entity.Pos.plane] = null;
            }
            HashSet<Entity> temp = _entities;
            _entities = entities;
            entities = temp;

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    queuedLocations[y * width + x] = 0;
                }
            }

            foreach (var kvp in queuedEntities) {
                if (kvp.Value.plane == (int)Tile.Plane.plant) {
                    kvp.Key.plant = true;
                }
                if (IsOccupied(kvp.Key.Pos)) {
                    var bruh = GetTile(kvp.Key.Pos).planes[kvp.Key.Pos.plane];
                    throw new AlreadyOccupiedException();
                }
                kvp.Key.Pos = kvp.Value;
                entities.Add(kvp.Key);
            }
            queuedEntities.Clear();

            stepCount++;
        }

        public byte[] Serialize() {
            ByteBuffer buffer = new ByteBuffer();

            // TODO(Alex): Write WorldState information here
            // ...
            BitHelper.WriteBytes(step, buffer);

            BitHelper.WriteBytes(worldStates.Count, buffer);
            foreach (KeyValuePair<string, BufferWriteable?> kvp in worldStates) {
                BitHelper.WriteBytes(kvp.Key, buffer);
                if (kvp.Value != null) BitHelper.WriteBytes(kvp.Value, buffer);
            }

            // Write tile and entities
            BitHelper.WriteBytes(width, buffer);
            BitHelper.WriteBytes(height, buffer);
            foreach (Tile tile in tiles) {
                BitHelper.WriteBytes(tile, buffer);
            }
            BitHelper.WriteBytes(entities.Count, buffer);
            foreach (Entity entity in entities) {
                BitHelper.WriteBytes(entity.Type, buffer);
                BitHelper.WriteBytes(entity.id, buffer);
                BitHelper.WriteBytes(entity.Pos, buffer);
                BitHelper.WriteBytes(entity.direction, buffer);
                BitHelper.WriteBytes(entity.state, buffer);
                BitHelper.WriteBytes(entity, buffer);
            }

            return buffer.Shrink();
        }
    }
}
