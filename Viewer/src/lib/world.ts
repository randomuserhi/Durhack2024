
class World {
    width: number;
    height: number;

    entities: Entity[];
    tiles: Tile[];

    constructor(width: number, height: number) {
        this.width = width;
        this.height = height;
    }
}

export interface Vec3 {
    x: number,
    y: number,
    plane: number
}

export class Entity {
    pos: Vec3;
}

export class Tile {
    pos: Vec3;
}