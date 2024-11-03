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
                state = "resting";
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
            if (pursuing != null) {
                if (TryPathTo(out Vec3 dir, pursuing.Pos)) {
                    if (Pos + dir == pursuing.Pos && Pos.plane == pursuing.Pos.plane || Pos.plane == pursuing.Pos.plane - 1) {
                        World.RemoveEntity(pursuing);
                        hunger += 75;
                        nextstate = "eat";
                        pursuing = null;
                    }
                    Pos += dir;
                }
            }
        }

        private void Eat() {
            delay = 20;
            nextstate = "wandering";
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
                        pursuing = closestEntity;
                    }
                }
            }
        }

        protected override void Update() {
            if (pursuing == null && Rand.Float(0, 1) < 0.05f) {
                FindBird();
            }

            --hunger;
            if (hunger < 50 || (pursuing != null && (pursuing.Pos - Pos).Mag() < 30)) {
                if (pursuing == null) {
                    FindBird();
                } else if (pursuing.remove == true) {
                    pursuing = null;
                }
                state = "pursuing";
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
            }
        }
    }
}
