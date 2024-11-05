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

            sim.tiles[0].AddState("fire", new IntState() { value = 1 });
            sim.tiles[0].AddState("temp", new IntState() { value = 60 });

            while (true) {

                sim.Step();
            }
        }
    }
}
