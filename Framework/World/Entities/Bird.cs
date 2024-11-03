namespace Biosphere {
    public class Bird : Entity {

        private int staminaCount = 20;
        private Entity? pursuing = null;

        public override string Type => "Bird";

        public Bird() : base() {
            state = "wandering";
        }

        private void Wandering() {
            --staminaCount;
            if (staminaCount <= 0) {
                state = "seeking";
            }
            delay = 1;
            Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), 1);
        }

        private void Fleeing() {
            // fleeing if detects a predator. can fly for a bit regardless of energy
            delay = 1;
        }

        private void Seeking() {
            delay = 3;
            Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), -1);
            if (Pos.plane == 0) {
                Pos += Vec3.transcend;
            }
        }

        private void Pursuing() {
            // a* for routing to worms
            delay = 1;

            // if catches worm:
            state = "wandering";
            staminaCount = 20;
        }

        private void FindWorm() {
            Entity? closestEntity = null;
            float closestDist = float.PositiveInfinity;

            foreach (Entity entity in World.entities) {
                if (entity.Type != "Worm" || entity.Pos.plane == 0) {
                    continue;
                }

                float dist = (entity.Pos - Pos).Mag();

                if (closestEntity != null) {
                    if (dist < closestDist) {
                        closestDist = dist;
                        closestEntity = entity;
                        state = "pursuing";
                    }

                } else if (dist < 7) {
                    closestDist = dist;
                    closestEntity = entity;
                    state = "pursuing";

                } else {
                    state = "seeking";
                }
            }
        }

        protected override void Update() {
            if (state == "seeking") {
                FindWorm();
            }

            switch (state) {
                case "wandering":
                    Wandering();
                    break;
                case "fleeing":
                    Fleeing();
                    break;
                case "seeking":
                    Seeking();
                    break;
                case "pursuing":
                    Pursuing();
                    break;
            }
        }
    }
}
