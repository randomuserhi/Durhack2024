namespace Biosphere {
    internal class WormSpawner : System {

        private float chance = 0.1f;

        public WormSpawner() : base() {
            delay = 0;
        }

        // me when I copy the tree spawner code
        protected override void Update() {
            delay = Rand.Int(10, 31);
            foreach (Tile tile in World.tiles) {
                chance = 0.05f;
                if (tile.planes[(int)Tile.Plane.underground] == null) {
                    if (tile.HasState("fire")) {
                        continue;
                    }
                    if (World.IsOccupied(new Vec3(tile.pos.x, tile.pos.y + World.width, (int)Tile.Plane.plant))) {
                        chance += 0.1f;
                    }
                    if (World.IsOccupied(new Vec3(tile.pos.x - 1, tile.pos.y, (int)Tile.Plane.plant))) {
                        chance += 0.1f;
                    }
                    if (World.IsOccupied(new Vec3(tile.pos.x + 1, tile.pos.y, (int)Tile.Plane.plant))) {
                        chance += 0.1f;
                    }
                    if (World.IsOccupied(new Vec3(tile.pos.x, tile.pos.y - World.width, (int)Tile.Plane.plant))) {
                        chance += 0.1f;
                    }

                    var randomValue = Rand.Float(0, 1);
                    if (randomValue <= chance) {
                        Console.WriteLine($"SPAWNED A worm AT {tile.pos.x} {tile.pos.y}");
                        World.AddEntity(new Tree(), new Vec3(tile.pos.x, tile.pos.y, (int)Tile.Plane.underground));
                    }
                }
            }
        }
    }
}
