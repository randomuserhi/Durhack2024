namespace Biosphere {
    public class Worm : Entity {

        private int staminaCount = 10;
        private Entity? pursuing = null;

        public override string Type => "Worm";

        public Worm() : base() {
            state = "wandering";
        }

        private void Wandering() {
            --staminaCount;
            if (staminaCount <= 4) {
                state = "idle";
            }
            delay = 2;
            Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), 0);
        }

        private void Resting() {
            delay = 20;
            staminaCount = 10;
        }

        private void Idle() {
            --staminaCount;
            if (staminaCount <= 0) {
                state = "resting";
            }
            delay = 4;
            Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), 0);
        }

        private void Pursuing() {
            // a* for chasing
        }

        private void FindEntity() {
            Entity? closestEntity = null;
            float closestDist = float.PositiveInfinity;

            foreach (Entity entity in World.entities) {
                if (entity.Type == "Dog" || entity.Pos.plane != this.Pos.plane) {
                    continue;
                }

                float dist = (entity.Pos - Pos).Mag();

                if (closestEntity != null) {
                    if (dist < closestDist) {
                        closestDist = dist;
                        closestEntity = entity;
                        state = "pursuing";
                    }

                } else if (dist < 5) {
                    closestDist = dist;
                    closestEntity = entity;
                    state = "pursuing";

                } else {
                    if (staminaCount <= 4) {
                        state = "idle";
                    } else {
                        state = "wandering";
                    }
                }
            }
        }

        protected override void Update() {
            if (state != "resting") {
                FindEntity();
            }

            switch (state) {
                case "wandering":
                    Wandering();
                    break;
                case "resting":
                    Resting();
                    break;
                case "idle":
                    Idle();
                    break;
                case "pursuing":
                    Pursuing();
                    break;
            }
        }
    }
}
