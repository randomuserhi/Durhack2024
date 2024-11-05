namespace Biosphere {
    public class Dog : Entity {
        private Entity? pursuing = null;

        public override string Type => "Dog";

        public int hunger = 150;

        public Dog() : base() {
            state = "wandering";
            energy = 20;
        }

        private void Wandering() {
            --energy;
            if (energy <= 0) {
                nextstate = "resting";
            }
            delay = 4;
            Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), 0);
        }

        private void Resting() {
            delay = 20;
            energy = 20;
            nextstate = "wandering";
        }

        private void Pursuing() {
            delay = 2;
            if (pursuing != null && !pursuing.remove) {
                TryPathTo(out Vec3 dir, pursuing.Pos, (Tile t) => !World.IsOccupied(t.pos));
                Pos += dir;
                if (Pos == pursuing.Pos && (pursuing.Pos.plane != (int)Tile.Plane.sky)) {
                    World.RemoveEntity(pursuing);
                    hunger += 30;
                    state = "eat";
                    pursuing = null;
                } else if ((Pos + dir) == pursuing.Pos && (pursuing.Pos.plane != (int)Tile.Plane.sky)) {
                    World.RemoveEntity(pursuing);
                    hunger += 30;
                    state = "eat";
                    pursuing = null;
                }
            } else {
                pursuing = null;
                state = "wandering";
                Wandering();
                FindBird();
            }
        }

        private int quota = 2;
        private void Eat() {
            delay = 20;
            nextstate = "wandering";

            if (--quota <= 0) {
                quota = 2;
                Vec3 pos = Pos + new Vec3(1, 0, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Dog(), pos);
                    return;
                }
                pos = Pos + new Vec3(-1, 0, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Dog(), pos);
                    return;
                }
                pos = Pos + new Vec3(0, 1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Dog(), pos);
                    return;
                }
                pos = Pos + new Vec3(0, -1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Dog(), pos);
                    return;
                }
                pos = Pos + new Vec3(1, 1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Dog(), pos);
                    return;
                }
                pos = Pos + new Vec3(1, -1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Dog(), pos);
                    return;
                }
                pos = Pos + new Vec3(-1, 1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Dog(), pos);
                    return;
                }
                pos = Pos + new Vec3(-1, -1, 0);
                if (!World.IsOccupied(pos) && World.ValidPos(pos)) {
                    World.AddEntity(new Dog(), pos);
                    return;
                }
            }
        }

        private void FindBird() {
            Entity? closestEntity = null;
            float closestDist = float.PositiveInfinity;

            foreach (Entity entity in World.entities) {
                if (entity.Type != "Bird") {
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
            pursuing = closestEntity;
        }

        protected override void Update() {
            if (pursuing == null && Rand.Float(0, 1) < 0.05f) {
                FindBird();
            }

            if (hunger > 0 && World.GetTile(Pos).HasState("fire")) {
                delay = 1;
                hunger /= 2;
                Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), 0);
            }

            --hunger;
            if (hunger < 50 || (pursuing != null && (pursuing.Pos - Pos).Mag() < 30)) {
                if (pursuing != null && pursuing.remove == true) {
                    pursuing = null;
                }
                if (pursuing == null) {
                    FindBird();
                }
                if (pursuing != null) state = "pursuing";
            }

            if (hunger <= 0) {
                delay = 30;
                state = "dead";

                if (hunger <= -15) {

                    Console.WriteLine("dead");
                    World.RemoveEntity(this);
                }
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
