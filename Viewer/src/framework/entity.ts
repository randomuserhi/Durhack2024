import { ByteStream } from "../lib/stream.js";
import { Vec3, World } from "../lib/world.js";

export class Entity {
    world: World;
    pos: Vec3;
    dir: Vec3;
    state: string = "";
    id: number;

    constructor(world: World) {
        this.pos = new Vec3();
        this.dir = new Vec3();
        this.world = world;
    }

    public update() {}

    public parse(data: ByteStream, id: number, state: string, pos: Vec3, dir: Vec3) {
        this.pos.copy(pos);
        this.dir.copy(dir);
        this.state = state;
        this.id = id;
    }

    public destroy() {}
}