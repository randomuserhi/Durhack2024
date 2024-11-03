namespace Biosphere {
    internal class CloudSpawner : System {

        public CloudSpawner() : base() {
            delay = 0;
        }

        private void SetClouds(int clusters, int val) {
            for (int i = 0; i < clusters; i++) {
                int x = Rand.Int(1, World.width - 1);
                int y = Rand.Int(1, World.height - 1);
                Console.WriteLine($"Trying a cluster at {x} {y}!");

                for (int j = -1; j < 2; j++) {
                    for (int k = -1; k < 2; k++) {
                        if (Rand.Float(0, 1) > 0.7) {
                            Tile tile = World.GetTile(new Vec3(x + j, y - +k));
                            if (!tile.HasState("cloud")) {
                                tile.AddState("cloud", new IntState() { value = val });
                            } else {
                                tile.SetState("cloud", new IntState() { value = val });
                            }
                        }
                    }
                }
            }
        }

        protected override void Update() {
            delay = Rand.Int(1, 9);
            SetClouds(Rand.Int(4, 8), 0);
            SetClouds(Rand.Int(3, 7), 1);
        }
    }
}
