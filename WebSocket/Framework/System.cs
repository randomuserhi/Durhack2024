namespace Framework {
    public abstract class System {
#pragma warning disable CS8618
        public Simulation World;
#pragma warning restore CS8618

        // Runs once every tick
        public abstract void Update();
    }
}
