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
            for (int i = 0; i < 1000; i++) {
                Console.WriteLine($"STEP!!!! {i}");
                Console.WriteLine($"dog: {dog.Pos.x} {dog.Pos.y} {dog.Pos.plane} {dog.remove}");
                Console.WriteLine($"worm: {worm.Pos.x} {worm.Pos.y} {worm.Pos.plane} {worm.remove}");
                Console.WriteLine($"bird: {bird.Pos.x} {bird.Pos.y} {bird.Pos.plane} {bird.remove}");
                sim.Step();
            }
        }
    }
}
