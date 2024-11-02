namespace Framework {

    public class Tile : BufferWriteable {
        // Enum for clearly indexing planes array
        public enum Plane {
            underground = 0,
            surface = 1,
            foliage = 2,
            sky = 3
        }

        // Dictionary to represent all the states of a given tile
        // 0 is false, 1 is true, above 1 is true and a variable specific to the state e.g. intenstity
        public Dictionary<string, BufferWriteable?> tileStates = new();

        public void SetState(string stateName, BufferWriteable value) {
            if (tileStates.ContainsKey(stateName)) {
                tileStates[stateName] = value;
            } else {
                throw new InvalidNameException();
            }
        }
        public void AddState(string stateName, BufferWriteable value) {
            if (tileStates.ContainsKey(stateName)) {
                throw new AlreadyExistsException();
            } else {
                tileStates[stateName] = value;
            }
        }

        public BufferWriteable? GetState(string stateName) {
            if (tileStates.ContainsKey(stateName)) {
                return tileStates[stateName];
            } else {
                throw new InvalidNameException();
            }
        }

        public bool HasState(string stateName) {
            return tileStates.ContainsKey(stateName);
        }

        // Planes array for storing what entities are on a given plane in a tile. Refer to Plane enum
        public Entity?[] planes = [null, null, null, null];

        // TODO(Alex)
        public void Write(ByteBuffer buffer) {
            BitHelper.WriteBytes(tileStates.Count, buffer);
            foreach (KeyValuePair<string, BufferWriteable?> kvp in tileStates) {
                BitHelper.WriteBytes(kvp.Key, buffer);
                if (kvp.Value != null) BitHelper.WriteBytes(kvp.Value, buffer);
            }
        }
    }
}