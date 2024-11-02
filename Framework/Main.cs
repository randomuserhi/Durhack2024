namespace Framework {
    public class Program {
        public class Dog : Entity {
            public override string Type => "Dog";

            public override void Update() {
                Pos += Vec3.up;
            }
        }

        // TODO:
        // - Auto fixing new positions such that if illlegal moves are made they simply dont happen (invalid plane, off edge of map)

        public static void Main() {
            Simulation sim = new Simulation(20, 20);
            var dog = new Dog();
            var dog2 = new Dog();
            sim.AddEntity(dog, Vec3.zero);
            sim.AddEntity(dog2, Vec3.up);
            for (int i = 0; i < 30; i++) {
                Console.WriteLine($"{dog.Pos.x} {dog.Pos.y} {dog.Pos.plane}"); // 0 0 0
                Console.WriteLine($"{dog2.Pos.x} {dog2.Pos.y} {dog2.Pos.plane}"); // 0 2 0
                sim.Step();
            }
        }
    }
}
