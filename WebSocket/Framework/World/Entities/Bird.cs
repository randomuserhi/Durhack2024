namespace Biosphere {
    public class Bird : Entity {

        public override string Type => "Bird";

        public int hunger = 100;

        public Bird() : base() {
            state = "wandering";
            energy = 10;
        }

        private void Wandering() {
            --energy;
            if (energy <= 0) {
                nextstate = "resting";
            }
            delay = 2;
            Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), 0);

            Vec3 temp = Pos;
            temp.plane = (int)Tile.Plane.sky;
            Pos = temp;
        }

        private void Resting() {
            delay = 30;
            energy = 10;
            state = "resting";
            nextstate = "wandering";

            Vec3 temp = Pos;
            temp.plane = (int)Tile.Plane.foliage;
            Pos = temp;

            temp = Pos;
            temp.plane = (int)Tile.Plane.surface;
            Pos = temp;

            if (Pos.plane == (int)Tile.Plane.sky) Wandering();
        }

        private Entity? corpse = null;

        private void Pursuing() {
            delay = 2;
            if (corpse != null && !corpse.remove) {
                TryPathTo(out Vec3 dir, corpse.Pos, (Tile t) => (t.pos.x == corpse.Pos.x && t.pos.y == corpse.Pos.y) || !World.IsOccupied(t.pos));

                Pos += dir;
                if (Pos == corpse.Pos) {
                    ((Dog)corpse).hunger -= 2;
                    hunger += 50;
                    nextstate = "eat";
                    corpse = null;
                }
            } else {
                corpse = null;
                Console.WriteLine("div");
                state = "wandering";
                Wandering();
                FindCorpse();
            }
        }

        private void FindCorpse() {
            Entity? closestEntity = null;
            float closestDist = float.PositiveInfinity;

            foreach (Entity entity in World.entities) {
                if (entity.Type != "Dog" || (entity is Dog && ((Dog)entity).hunger > 0)) {
                    continue;
                }

                float dist = (entity.Pos - Pos).Mag();

                if (closestEntity != null) {
                    if (dist < closestDist) {
                        closestDist = dist;
                        closestEntity = entity;
                    }
                } else {
                    closestEntity = entity;
                }
            }
            corpse = closestEntity;
        }

        private Entity? dog = null;
        private void FindClosestDog() {
            Entity? closestEntity = null;
            float closestDist = float.PositiveInfinity;

            foreach (Entity entity in World.entities) {
                if (entity.Type != "Dog" || (entity is Dog && ((Dog)entity).hunger < 0)) {
                    continue;
                }

                float dist = (entity.Pos - Pos).Mag();

                if (closestEntity != null) {
                    if (dist < closestDist) {
                        closestDist = dist;
                        closestEntity = entity;
                    }
                } else {
                    closestEntity = entity;
                }
            }
            dog = closestEntity;
        }

        private int quota = 3;
        private void Eat() {
            delay = 15;
            state = "eat";
            nextstate = "wandering";

            Vec3 temp = Pos;
            temp.plane = (int)Tile.Plane.foliage;
            Pos = temp;

            temp = Pos;
            temp.plane = (int)Tile.Plane.surface;
            Pos = temp;

            if (--quota <= 0) {
                quota = 3;
                Vec3 pos = Pos + new Vec3(1, 0, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Bird(), pos);
                    return;
                }
                pos = Pos + new Vec3(-1, 0, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Bird(), pos);
                    return;
                }
                pos = Pos + new Vec3(0, 1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Bird(), pos);
                    return;
                }
                pos = Pos + new Vec3(0, -1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Bird(), pos);
                    return;
                }
                pos = Pos + new Vec3(1, 1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Bird(), pos);
                    return;
                }
                pos = Pos + new Vec3(1, -1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Bird(), pos);
                    return;
                }
                pos = Pos + new Vec3(-1, 1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Bird(), pos);
                    return;
                }
                pos = Pos + new Vec3(-1, -1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Bird(), pos);
                    return;
                }
            }

            if (Pos.plane == (int)Tile.Plane.sky) Wandering();
        }

        protected override void Update() {
            FindClosestDog();
            if (dog != null) {
                Vec3 diff = dog.Pos - Pos;
                diff.plane = 0;
                if (diff.x == 0 && diff.y == 0) {
                    Wandering();
                    delay = 2;
                } else if ((diff).Mag() < 5 && hunger > 30) {
                    if (Rand.Float(0, 1) < 0.05f) {
                        Resting();
                    }

                    Pos -= diff.Norm();
                    Vec3 temp = Pos;
                    temp.plane = (int)Tile.Plane.sky;
                    Pos = temp;
                    delay = 2;
                    return;
                }
            }

            if (corpse == null && Rand.Float(0, 1) < 0.05f) {
                FindCorpse();
            }

            if (hunger > 0 && World.GetTile(Pos).HasState("fire")) {
                delay = 1;
                hunger /= 2;
                Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), 0);
            }

            --hunger;
            if (hunger < 30 || (corpse != null && (corpse.Pos - Pos).Mag() < 30)) {
                if (corpse != null && corpse.remove == true) {
                    corpse = null;
                }
                if (corpse == null) {
                    FindCorpse();
                }
                if (corpse != null) state = "pursuing";
            }

            if (hunger < 0) {
                World.RemoveEntity(this);
            }

            switch (state) {
            case "wandering":
                Wandering();
                break;
            case "resting":
                Resting();
                break;
            case "pursuing":
                Pursuing();
                break;
            case "eat":
                Eat();
                break;
            }
        }
    }
}
