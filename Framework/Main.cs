namespace Biosphere {
    public class Program {

        public static void Main() {
            Simulation sim = new Simulation(20, 20);

            // Generate world...
            sim.AddSystem(new TreeSpawner());
            sim.AddSystem(new BirdSpawner());
            sim.AddSystem(new DogSpawner());

            sim.AddRule(new Fire());
            sim.AddRule(new Rain());

            while (true) {
                sim.Step();
            }
        }
    }
}
