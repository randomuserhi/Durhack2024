﻿namespace Biosphere {
    public class Tree : Entity {

        public int growthState;
        private int clock = 0;
        private static string[] states = ["sprout", "sapling", "mature", "ancient", "decaying"];
        private static int[] time = [50, 75, 300, 500, 700];

        public override string Type => "Tree";

        public Tree() : base() {
            growthState = 0;
            state = states[0];
        }

        protected override void Update() {
            ++clock;
            if (clock >= time[growthState]) {
                ++growthState;
                clock = 0;
            }

            if (growthState > 4) {
                World.RemoveEntity(this);
            } else {
                state = states[growthState];
            }
        }
    }
}
