namespace Biosphere {
    internal class BirdSpawner : System {

        private float chance = 0.001f;

        public BirdSpawner() : base() {
            delay = 0;
        }

        protected override void Update() {
            delay = Rand.Int(5, 11);
            foreach (Tile tile in World.tiles) {
                if (tile.planes[(int)Tile.Plane.sky] == null) {
                    if (tile.HasState("fire")) {
                        continue;
                    }
                    float cchance = chance;

                    Entity? e = tile.planes[(int)Tile.Plane.plant];
                    if (e != null && e is Tree && ((Tree)e).growthState > 2 && ((Tree)e).growthState < 4) {
                        cchance += 0.01f;
                    }

                    var randomValue = Rand.Float(0, 1);
                    if (randomValue <= cchance) {
                        Console.WriteLine($"SPAWNED A bird AT {tile.pos.x} {tile.pos.y}");
                        World.AddEntity(new Bird(), new Vec3(tile.pos.x, tile.pos.y, (int)Tile.Plane.sky));
                    }
                }
            }

            chance = 0.0001f;
        }
    }
}
