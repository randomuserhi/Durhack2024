namespace Biosphere {
    internal class DogSpawner : System {

        private float chance = 0.001f;

        public DogSpawner() : base() {
            delay = 0;
        }

        protected override void Update() {
            delay = Rand.Int(5, 11);
            foreach (Tile tile in World.tiles) {
                if (tile.planes[(int)Tile.Plane.surface] == null) {
                    if (tile.HasState("fire")) {
                        continue;
                    }
                    var randomValue = Rand.Float(0, 1);
                    if (randomValue <= chance) {
                        Console.WriteLine($"SPAWNED A dog AT {tile.pos.x} {tile.pos.y}");
                        World.AddEntity(new Dog(), new Vec3(tile.pos.x, tile.pos.y, (int)Tile.Plane.surface));
                    }
                }
            }

            chance = 0.0001f;
        }
    }
}
