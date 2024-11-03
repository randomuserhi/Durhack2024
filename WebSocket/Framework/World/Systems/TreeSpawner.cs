namespace Biosphere {
    internal class TreeSpawner : System {

        private float chance = 0.2f;

        public TreeSpawner() : base() {
            delay = 10;
        }

        protected override void Update() {
            foreach (Tile tile in World.tiles) {
                if (tile.planes[(int)Tile.Plane.plant] == null) {
                    if (tile.HasState("fire")) {
                        continue;
                    }
                    Vec3 v = new Vec3(tile.pos.x, tile.pos.y + World.width, (int)Tile.Plane.plant);
                    if (World.IsOccupied(v)) {
                        Entity? e = World.GetTile(v).planes[(int)Tile.Plane.plant];
                        if (e is Tree && ((Tree)e).growthState > 1) {
                            chance += 0.1f;
                        }
                    }
                    v = new Vec3(tile.pos.x, tile.pos.y + World.width, (int)Tile.Plane.plant);
                    if (World.IsOccupied(v)) {
                        Entity? e = World.GetTile(v).planes[(int)Tile.Plane.plant];
                        if (e is Tree && ((Tree)e).growthState > 1) {
                            chance += 0.1f;
                        }
                    }
                    v = new Vec3(tile.pos.x, tile.pos.y + World.width, (int)Tile.Plane.plant);
                    if (World.IsOccupied(v)) {
                        Entity? e = World.GetTile(v).planes[(int)Tile.Plane.plant];
                        if (e is Tree && ((Tree)e).growthState > 1) {
                            chance += 0.1f;
                        }
                    }
                    v = new Vec3(tile.pos.x, tile.pos.y + World.width, (int)Tile.Plane.plant);
                    if (World.IsOccupied(v)) {
                        Entity? e = World.GetTile(v).planes[(int)Tile.Plane.plant];
                        if (e is Tree && ((Tree)e).growthState > 1) {
                            chance += 0.1f;
                        }
                    }

                    var randomValue = Rand.Float(0, 1);
                    if (randomValue <= chance) {
                        Console.WriteLine($"SPAWNED A TREE AT {tile.pos.x} {tile.pos.y}");
                        World.AddEntity(new Tree(), new Vec3(tile.pos.x, tile.pos.y, (int)Tile.Plane.plant));
                    }

                    chance = 0.001f;
                }
            }
        }
    }
}
