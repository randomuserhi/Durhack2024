using System.Runtime.Serialization;

namespace Biosphere {
    public class Tree : Entity {

        private int growthState;
        private static string[] states = ["sprout", "sapling", "Mature Tree", "Ancient Tree", "Decaying Tree"];

        public override string Type => "Tree";

        public Tree() : base() {
            growthState = 0;
            state = "sapling";
        }

        protected override void Update() {
            ++growthState;
            if (growthState % 10 == 0) {
                if (growthState == 50) {
                    World.RemoveEntity(this);
                }
                state = states[growthState / 10];
            }
        }
    }
}
