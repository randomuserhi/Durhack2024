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
                state = "resting";
            }
            delay = 2;
            Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), 0);
        }

        private void Resting() {
            delay = 30;
            energy = 10;
            nextstate = "wandering";
            Vec3 temp = Pos;
            temp.plane = (int)Tile.Plane.surface;
            Pos = temp;
        }

        private Entity? corpse = null;

        private void Pursuing() {
            delay = 2;
            if (corpse != null) {
                if (TryPathTo(out Vec3 dir, corpse.Pos)) {
                    if (Pos + dir == corpse.Pos) {
                        ((Dog)corpse).hunger -= 2;
                        hunger += 50;
                        state = "eat";
                        corpse = null;
                    }
                    Pos += dir;
                }
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
                        corpse = closestEntity;
                    }
                }
            }
        }

        private void Eat() {
            delay = 15;
            nextstate = "wandering";
        }

        protected override void Update() {
            if (corpse == null && Rand.Float(0, 1) < 0.05f) {
                FindCorpse();
            }

            --hunger;
            if (hunger < 30 || (corpse != null && (corpse.Pos - Pos).Mag() < 30)) {
                if (corpse == null) {
                    FindCorpse();
                } else if (corpse.remove == true) {
                    corpse = null;
                }
                state = "pursuing";
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
