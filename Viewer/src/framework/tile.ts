import * as BitHelper from "../lib/bithelper.js";
import { LoadImage, Sprite } from "../lib/renderer.js";
import { ByteStream } from "../lib/stream.js";
import { Vec3, World } from "../lib/world.js";

const ground = LoadImage("./images/tiles/ground.png");
const cloud = LoadImage("./images/tiles/cloud.png");
const fire = LoadImage("./images/tiles/fire.png");

export class Tile {
    world: World;

    pos: Vec3;

    ground: Sprite;
    cloud: Sprite;
    fire: Sprite;

    states = new Map<string, any>();


    constructor(world: World, pos: Vec3) {
        this.world = world;
        
        this.pos = new Vec3().copy(pos);

        this.ground = new Sprite(ground, 35, 35);
        this.ground.layer = -2;

        this.cloud = new Sprite(cloud, 30, 30);
        this.cloud.layer = 10;
        this.cloud.offset.y = -this.world.tileSize * 0.75;
        
        this.fire = new Sprite(fire, 30, 30);
        this.fire.layer = 10;

        this.world.renderer.addSprite(this.ground);
    
        this.update();
    }

    public update() {
        this.ground.pos.x = this.pos.x * this.world.tileSize * 0.5; 
        this.ground.pos.y = this.pos.y * this.world.tileSize * 0.5;

        if (this.states.has("cloud") && (this.states.get("cloud")! as number) == 1) {
            this.world.renderer.addSprite(this.cloud);
        } else {
            this.world.renderer.removeSprite(this.cloud);
        }
        this.cloud.pos.x = this.pos.x * this.world.tileSize * 0.5; 
        this.cloud.pos.y = this.pos.y * this.world.tileSize * 0.5;

        if (this.states.has("fire") && (this.states.get("fire")! as number) == 1) {
            this.world.renderer.addSprite(this.fire);
        } else {
            this.world.renderer.removeSprite(this.fire);
        }
        this.fire.pos.x = this.pos.x * this.world.tileSize * 0.5; 
        this.fire.pos.y = this.pos.y * this.world.tileSize * 0.5;
    }

    public parse(data: ByteStream) {
        this.states.clear();
        const numStates = BitHelper.readInt(data);
        for (let i = 0; i < numStates; ++i) {
            const state = BitHelper.readString(data);
            switch (state) {
            case "cloud": {
                this.states.set("cloud", BitHelper.readInt(data));
            } break;
            case "temp": {
                this.states.set("temp", BitHelper.readInt(data));
            } break;
            case "fire": {
                this.states.set("fire", BitHelper.readInt(data));
            } break;
            default: throw new Error(`${state} is not implemented.`);
            }
        }
    }
}