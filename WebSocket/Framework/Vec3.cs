using System.Runtime.CompilerServices;

namespace Framework {
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


        public float Mag(Vec3 vec1) {
            return (float)Math.Sqrt(MagSqrd(vec1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int MagSqrd(Vec3 vec1) {
            return (vec1.x * vec1.x) + (vec1.y * vec1.y) + (vec1.plane * vec1.plane); ;
        }
    }
}
