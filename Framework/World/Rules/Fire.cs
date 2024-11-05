namespace Biosphere {
    internal class Fire : Rule {

        public Fire() : base() { }

        public override void Each(Tile t) {
            if (!t.HasState("fire")) {
                return;
            }
            Vec3 tileBelow = new Vec3(t.pos.x, t.pos.y - 1, 0);
            Vec3 tileLeft = new Vec3(t.pos.x - 1, t.pos.y, 0);
            Vec3 tileRight = new Vec3(t.pos.x + 1, t.pos.y, 0);
            Vec3 tileAbove = new Vec3(t.pos.x, t.pos.y + 1, 0);

            if (World.ValidPos(tileBelow) && !World.GetTile(tileBelow).HasState("fire")) {
                if (!World.GetTile(tileBelow).HasState("temp")) {
                    World.GetTile(tileBelow).AddState("temp", new IntState() { value = 3 });
                } else {
                    ((IntState)World.GetTile(tileBelow).GetState("temp")!).value += 3;
                }
            }

            var randomValue = Rand.Float(0, 1);
            if (World.ValidPos(tileBelow) && randomValue <= (0.1f + (((IntState)World.GetTile(tileBelow).GetState("temp")!).value) / 100)) {
                if (!World.GetTile(tileBelow).HasState("fire")) World.GetTile(tileBelow).AddState("fire", new IntState() { value = 1 });
                if (!World.GetTile(tileBelow).HasState("temp")) {
                    World.GetTile(tileBelow).SetState("temp", new IntState() { value = 60 });
                }
            }

            if (World.ValidPos(tileAbove) && !World.GetTile(tileAbove).HasState("fire")) {
                if (!World.GetTile(tileAbove).HasState("temp")) {
                    World.GetTile(tileAbove).AddState("temp", new IntState() { value = 3 });
                } else {
                    ((IntState)World.GetTile(tileAbove).GetState("temp")!).value += 3;
                }
            }

            randomValue = Rand.Float(0, 1);
            if (World.ValidPos(tileAbove) && randomValue <= (0.1f + (((IntState)World.GetTile(tileAbove).GetState("temp")!).value) / 100)) {
                if (!World.GetTile(tileAbove).HasState("fire")) World.GetTile(tileAbove).AddState("fire", new IntState() { value = 1 });
                if (!World.GetTile(tileAbove).HasState("temp")) {
                    World.GetTile(tileAbove).SetState("temp", new IntState() { value = 60 });
                }
            }

            if (World.ValidPos(tileRight) && !World.GetTile(tileRight).HasState("fire")) {
                if (!World.GetTile(tileRight).HasState("temp")) {
                    World.GetTile(tileRight).AddState("temp", new IntState() { value = 3 });
                } else {
                    ((IntState)World.GetTile(tileRight).GetState("temp")!).value += 3;
                }
            }

            randomValue = Rand.Float(0, 1);
            if (World.ValidPos(tileRight) && randomValue <= (0.1f + (((IntState)World.GetTile(tileRight).GetState("temp")!).value) / 100)) {
                if (!World.GetTile(tileRight).HasState("fire")) World.GetTile(tileRight).AddState("fire", new IntState() { value = 1 });
                if (!World.GetTile(tileRight).HasState("temp")) {
                    World.GetTile(tileRight).SetState("temp", new IntState() { value = 60 });
                }
            }

            if (World.ValidPos(tileLeft) && !World.GetTile(tileLeft).HasState("fire")) {
                if (!World.GetTile(tileLeft).HasState("temp")) {
                    World.GetTile(tileLeft).AddState("temp", new IntState() { value = 3 });
                } else {
                    ((IntState)World.GetTile(tileLeft).GetState("temp")!).value += 3;
                }
            }

            randomValue = Rand.Float(0, 1);
            if (World.ValidPos(tileLeft) && randomValue <= (0.1f + (((IntState)World.GetTile(tileLeft).GetState("temp")!).value) / 100)) {
                if (!World.GetTile(tileLeft).HasState("fire")) World.GetTile(tileLeft).AddState("fire", new IntState() { value = 1 });
                if (!World.GetTile(tileLeft).HasState("temp")) {
                    World.GetTile(tileLeft).SetState("temp", new IntState() { value = 60 });
                }
            }


        }
    }


}
