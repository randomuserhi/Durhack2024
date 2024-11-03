namespace Biosphere {
    public class Bird : Entity {

        private int staminaCount = 20;
        private int fleeCount = 0;
        private Entity? pursuing = null;
        private Entity? fleeing = null;

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
            --fleeCount;
            if (fleeing != null) {
                int x_dif = Pos.x - fleeing.Pos.x;
                int y_dif = Pos.y - fleeing.Pos.y;
                if (Math.Abs(x_dif) > Math.Abs(y_dif)) {
                    Pos += new Vec3(Math.Sign(x_dif) * 1, 0, Rand.Int(0, 2));
                } else {
                    Pos += new Vec3(0, Math.Sign(y_dif) * 1, Rand.Int(0, 2));
                }
                if (fleeCount <= 0) {
                    if (staminaCount > 0) {
                        state = "wandering";
                    } else {
                        state = "seeking";
                    }
                }
            }
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
            delay = 1;
            if (pursuing != null) {
                if (Pos.plane > 1) {
                    Pos += Vec3.descend;
                }
                if (TryPathTo(out Vec3 dir, pursuing.Pos)) {
                    if (Pos + dir == pursuing.Pos && Pos.plane == pursuing.Pos.plane) {
                        World.RemoveEntity(pursuing);
                        pursuing = null;
                    }
                    Pos += dir;
                }
            }
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

        private void CheckDog() {
            Entity? closestEntity = null;
            float closestDist = float.PositiveInfinity;

            foreach (Entity entity in World.entities) {
                if (entity.Type != "Dog" || entity.Pos.plane == 0) {
                    continue;
                }

                float dist = (entity.Pos - Pos).Mag();

                if (closestEntity != null) {
                    if (dist < closestDist) {
                        closestDist = dist;
                        closestEntity = entity;
                        state = "fleeing";
                        fleeing = closestEntity;
                        fleeCount = 4;
                    }

                } else if (dist < 4) {
                    closestDist = dist;
                    closestEntity = entity;
                    state = "fleeing";
                    fleeing = closestEntity;
                    fleeCount = 4;

                } else {
                    state = "seeking";
                }
            }
        }

        protected override void Update() {
            if (state == "seeking") {
                FindWorm();
                CheckDog();
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
