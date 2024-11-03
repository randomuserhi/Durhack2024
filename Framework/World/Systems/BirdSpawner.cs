namespace Biosphere {
    internal class BirdSpawner : System {

        private float chance = 0.005f;
        private bool firstBatch = true;

        public BirdSpawner() : base() {
            delay = 0;
        }

        protected override void Update() {
            delay = Rand.Int(5, 11);
            if (firstBatch) {
                firstBatch = false;
                foreach (Tile tile in World.tiles) {
                    chance = 0.005f;
                    if (tile.planes[(int)Tile.Plane.sky] == null) {
                        if (tile.HasState("fire")) {
                            continue;
                        }
                        var randomValue = Rand.Float(0, 1);
                        if (randomValue <= chance) {
                            Console.WriteLine($"SPAWNED A bird AT {tile.pos.x} {tile.pos.y}");
                            World.AddEntity(new Bird(), new Vec3(tile.pos.x, tile.pos.y, (int)Tile.Plane.sky));
                        }
                    }
                }
            } else {
                int birdCount = 0;
                int birdEnergySum = 0;
                foreach (Entity entity in World.entities) {
                    if (entity.Type == "Bird") {
                        birdCount++;
                        birdEnergySum += entity.energy;
                    }
                }

                int offspringNum;
                if (birdCount > 0) {
                    offspringNum = birdCount * (birdEnergySum / (birdCount * 20));
                } else {
                    offspringNum = 0;
                }



                for (int i = 0; i < offspringNum; ++i) {
                    int x = Rand.Int(0, World.width);
                    int y = Rand.Int(0, World.height);
                    if (!World.IsOccupied(new Vec3(x, y, (int)Tile.Plane.sky))) {
                        World.AddEntity(new Bird(), new Vec3(x, y, (int)Tile.Plane.sky));
                        Console.WriteLine($"SPAWNED A bird AT {x} {y}");
                    }
                }
            }
        }
    }
}
