namespace Biosphere {
    internal class Rain : Rule {

        public Rain() : base() { }

        public override void Each(Tile t) {
            if (!t.HasState("cloud")) {
                return;
            }

            if (t.HasState("temp")) {
                t.SetState("temp", new IntState() { value = ((IntState)t.GetState("temp")).value - 2 });
            } else {
                t.AddState("temp", new IntState() { value = -2 });
            }

            if (t.HasState("fire")) {
                t.SetState("fire", new IntState() { value = 0 });
            }
        }
    }
}
