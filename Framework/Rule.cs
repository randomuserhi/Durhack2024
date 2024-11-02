namespace Framework {
    public abstract class Rule {
#pragma warning disable CS8618
        public Simulation World;
#pragma warning restore CS8618

        public abstract void Each(Tile t);
    }
}
