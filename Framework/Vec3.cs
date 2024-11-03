using System.Runtime.CompilerServices;

namespace Biosphere {
    public struct Vec3 {
        public int x;
        public int y;
        public int plane;

        public static Vec3 zero = new Vec3(0, 0, 0);
        public static Vec3 up = new Vec3(0, 1, 0);
        public static Vec3 down = new Vec3(0, -1, 0);
        public static Vec3 right = new Vec3(1, 0, 0);
        public static Vec3 left = new Vec3(-1, 0, 0);
        public static Vec3 transcend = new Vec3(0, 0, 1);
        public static Vec3 descend = new Vec3(0, 0, -1);

        public Vec3(int x, int y, int plane) {
            this.x = x;
            this.y = y;
            this.plane = plane;
        }

        public static Vec3 operator +(Vec3 vec1, Vec3 vec2) {
            return new Vec3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.plane + vec2.plane);
        }

        public static Vec3 operator -(Vec3 vec1, Vec3 vec2) {
            return new Vec3(vec1.x - vec2.x, vec1.y - vec2.y, vec1.plane - vec2.plane);
        }


        public static Vec3 operator *(Vec3 vec1, int multi) {
            return new Vec3(vec1.x * multi, vec1.y * multi, vec1.plane * multi);
        }

        /*
        public Vec3 Div(Vec3 vec1, int divi)
        {
            return new Vec3(vec1.x / divi, vec1.y / divi, vec1.plane / divi);
        }
        */

        public Vec3 Norm() {
            float magnitude = Mag();
            return new Vec3((int)Math.Round(x / magnitude), (int)Math.Round(y / magnitude), plane);
        }


        public float Mag() {
            return (float)Math.Sqrt(MagSqrd());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int MagSqrd() {
            return (x * x) + (y * y);
        }
    }
}
