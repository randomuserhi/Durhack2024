namespace Biosphere {
    public class Simulation {
        public Dictionary<string, BufferWriteable?> worldStates = new();

        public Tile[] tiles;
        public List<System> systems = new();
        public List<Rule> rules = new();
        public List<Entity> entities = new();

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
                entity.Pos = pos;
                GetTile(entity.Pos).planes[entity.Pos.plane] = entity;
                entities.Add(entity);
            }
        }

        public void RemoveEntity<T>(T entity) where T : Entity {
            GetTile(entity.Pos).planes[entity.Pos.plane] = null;
            entities.Remove(entity);
        }

        public Tile GetTile(Vec3 vec) {
            return tiles[vec.y * width + vec.x];
        }

        public bool IsOccupied(Vec3 vec) {
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
                system.Update();
            }

            foreach (Entity entity in entities) {
                entity.Step();
            }
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
            BitHelper.WriteBytes(size, buffer);
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
