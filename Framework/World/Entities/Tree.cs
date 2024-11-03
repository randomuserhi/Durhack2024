using System.Xml.Serialization;

namespace Biosphere {
    public class Tree : Entity {

        private int growthState;
        private static string[] states = ["sprout", "sapling", "mature", "ancient", "decaying"];

        public override string Type => "Tree";

        public Tree() : base() {
            growthState = 0;
            state = "sapling";
        }

        public override void Write(ByteBuffer buffer) {
            BitHelper.WriteBytes(new Vec3(this.Pos.x, this.Pos.y, 4), buffer);
        }

        protected override void Update() {
            ++growthState;
            if (growthState % 10 == 0) {
                if (growthState == 50) {
                    World.RemoveEntity(this);
                }
                state = states[growthState / 10 - 1];
            }
        }
    }
}
