namespace Biosphere {
    public class Program {

        public static void Main() {
            Simulation sim = new Simulation(20, 20);
            var dog = new Dog();
            sim.AddEntity(dog, Vec3.zero);
            for (int i = 0; i < 30; i++) {
                Console.WriteLine($"{dog.Pos.x} {dog.Pos.y} {dog.Pos.plane}"); // 0 0 0
                sim.Step();
            }
        }
    }
}
