namespace Biosphere {
    internal class DogSpawner : System {

        private float chance = 0.005f;
        private bool firstBatch = true;

        public DogSpawner() : base() {
            delay = 0;
        }

        protected override void Update() {
            delay = Rand.Int(5, 11);
            if (firstBatch) {
                firstBatch = false;
                foreach (Tile tile in World.tiles) {
                    chance = 0.005f;
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
            } else {
                int dogCount = 0;
                int dogEnergySum = 0;
                foreach (Entity entity in World.entities) {
                    if (entity.Type == "Dog") {
                        dogCount++;
                        dogEnergySum += entity.energy;
                    }
                }

                int offspringNum;
                if (dogCount > 0) {
                    offspringNum = dogCount * (dogEnergySum / (dogCount * 20));
                } else {
                    offspringNum = 0;
                }



                for (int i = 0; i < offspringNum; ++i) {
                    int x = Rand.Int(0, World.width);
                    int y = Rand.Int(0, World.height);
                    if (!World.IsOccupied(new Vec3(x, y, (int)Tile.Plane.surface))) {
                        World.AddEntity(new Dog(), new Vec3(x, y, (int)Tile.Plane.surface));
                        Console.WriteLine($"SPAWNED A dog AT {x} {y}");
                    }
                }
            }
        }
    }
}
