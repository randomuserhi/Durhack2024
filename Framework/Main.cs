namespace Biosphere {
    public class Program {

        public static void Main() {
            Simulation sim = new Simulation(20, 20);
            var dog = new Dog();
            var bird = new Bird();
            var worm = new Worm();
            sim.AddEntity(dog, new Vec3(2, 2, 1));
            sim.AddEntity(bird, new Vec3(2, 5, 3));
            sim.AddEntity(worm, new Vec3(2, 2, 0));
            sim.AddSystem(new CloudSpawner());
            sim.AddSystem(new TreeSpawner());
            sim.AddSystem(new WormSpawner());
            sim.AddSystem(new BirdSpawner());
            sim.AddSystem(new DogSpawner());
            for (int i = 0; i < 1000; i++) {
                Console.WriteLine($"STEP!!!! {i}");
                sim.Step();
            }
        }
    }
}
