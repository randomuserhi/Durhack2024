namespace Biosphere {
    public class Dog : Entity {
        public override string Type => "Dog";

        public override void Update() {
            Pos += PathTo(new Vec3(6, 6, 0), (t) => t.pos.x % 2 == 0 && t.pos.y % 2 == 0);
        }
    }
}
