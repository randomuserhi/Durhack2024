namespace Biosphere {
    internal class TreeSpawner : System {

        private float chance = 0.2f;

        public TreeSpawner() : base() {
            delay = 10;
        }

        protected override void Update() {
            foreach (Tile tile in World.tiles) {
                chance = 0.01f;
                if (tile.planes[(int)Tile.Plane.plant] == null) {
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
                        Console.WriteLine($"SPAWNED A TREE AT {tile.pos.x} {tile.pos.y}");
                        World.AddEntity(new Tree(), new Vec3(tile.pos.x, tile.pos.y, (int)Tile.Plane.plant));
                    }
                }
            }
        }
    }
}
