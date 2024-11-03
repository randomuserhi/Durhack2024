import { LoadImage, Sprite } from "../../lib/renderer.js";
import { World } from "../../lib/world.js";
import { Entity } from "../entity.js";

const alive = LoadImage("./images/entities/wolf.png");
const dead = LoadImage("./images/entities/wolf-dead.png");
const eat = LoadImage("./images/entities/wolf-eat.png");

export class Dog extends Entity {
    sprite: Sprite;

    constructor(world: World) {
        super(world);

        this.sprite = new Sprite(alive, world.tileSize, world.tileSize);
        this.sprite.layer = -1;

        this.world.renderer.addSprite(this.sprite);

        this.update();
    }

    public update() {
        if (this.state === "dead") {
            this.sprite.image = dead;
        } else if (this.state === "eat") {
            this.sprite.image = eat;
        } else {
            this.sprite.image = alive;
        }

        this.sprite.pos.x = this.pos.x * this.world.tileSize * 0.5; 
        this.sprite.pos.y = this.pos.y * this.world.tileSize * 0.5;
    }

    public destroy(): void {
        this.world.renderer.removeSprite(this.sprite);
    }
}