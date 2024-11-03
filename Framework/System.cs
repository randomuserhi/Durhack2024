namespace Biosphere {
    public abstract class System {
#pragma warning disable CS8618
        public Simulation World;
#pragma warning restore CS8618

        // Runs once every tick
        protected abstract void Update();

        public int delay = 0;
        private int _delay = 0;
        public void Step() {
            if (_delay <= 0) {
                _delay = delay;
                Update();
            }
            --_delay;
        }
    }
}
