namespace Biosphere {
    public class Worm : Entity {

        private int staminaCount = 5;
        private Entity? pursuing = null;

        public override string Type => "Worm";

        public Worm() : base() {
            state = "wandering";
        }

        private void Wandering() {
            --staminaCount;
            if (staminaCount <= 0) {
                if (Rand.Int(0, 2) == 0) {
                    state = "chilling";
                } else {
                    state = "sunbathing";
                }
            }
            delay = 0;
            Pos += new Vec3(Rand.Int(-1, 2), Rand.Int(-1, 2), -1);
        }

        private void Chilling() {
            delay = 20;
            staminaCount = 5;
            state = "wandering";
        }

        private void Sunbathing() {
            Pos += Vec3.transcend;
            delay = 15;
            staminaCount = 5;
            state = "wandering";
        }

        protected override void Update() {

            switch (state) {
                case "wandering":
                    Wandering();
                    break;
                case "chilling":
                    Chilling();
                    break;
                case "sunbathing":
                    Sunbathing();
                    break;
            }
        }
    }
}
