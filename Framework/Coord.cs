namespace Biosphere {
    public class Coord {
        // Coord is automatically instantiated when an entity is instantiated
        // Plane number refers to a specific plane - refer to Plane enum in the Tile class
        int x;
        int y;
        int plane;

        public Coord(int x = 0, int y = 0, int plane = 1) {
            this.x = x;
            this.y = y;
            this.plane = plane;
        }
    }
}
