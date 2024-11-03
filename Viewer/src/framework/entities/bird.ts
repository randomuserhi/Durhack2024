import { LoadImage, Sprite } from "../../lib/renderer.js";
import { World } from "../../lib/world.js";
import { Entity } from "../entity.js";

const ancient = LoadImage("./images/entities/bird.png");
const rest = LoadImage("./images/entities/bird-rest.png");

const offsets = [
    0,
    0,
    0.2,
    0.75
];

export class Bird extends Entity {
    sprite: Sprite;

    constructor(world: World) {
        super(world);

        this.sprite = new Sprite(ancient, world.tileSize, world.tileSize);
        this.sprite.layer = -1;

        this.world.renderer.addSprite(this.sprite);

        this.update();
    }

    public update() {
        if (this.state === "resting" || this.state === "eat") {
            this.sprite.image = rest;
        } else {
            this.sprite.image = ancient;
        }

        this.sprite.offset.y = -this.world.tileSize * offsets[this.pos.plane];
        this.sprite.pos.x = this.pos.x * this.world.tileSize * 0.5; 
        this.sprite.pos.y = this.pos.y * this.world.tileSize * 0.5;
    }

    public destroy(): void {
        this.world.renderer.removeSprite(this.sprite);
    }
}