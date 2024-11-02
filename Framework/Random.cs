namespace Biosphere {
    public class Rand {
        private static Random rnd = new Random();

        public static float Float(int lb, int ub) {
            if (lb > ub) {
                throw new ArgumentException("Lower bound cannot be greater than upper bound!");
            } else {
                return (ub - lb) * (float)rnd.NextDouble() + lb;
            }
        }

        public static float Int(int lb, int ub) {
            if (lb > ub) {
                throw new ArgumentException("Lower bound cannot be greater than upper bound!");
            } else {
                return rnd.Next(lb, ub);
            }
        }
    }
}
